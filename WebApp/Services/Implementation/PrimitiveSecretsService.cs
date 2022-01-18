using Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Repository.Infrastructure;
using Services.DTO.User;
using Services.Infrastructure;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Services.Implementation
{
    //TODO lepsze wyjatki i sprawdzanie przypadkow
    public class PrimitiveSecretsService : ISecretsService
    {
        /// <summary>
        /// Primitive secret service, uses master password hash as a key
        /// for ciphering/deciphering service passwords
        /// It also creates hashes of the same length for both user password and master password
        /// </summary>
        private readonly int iterationPbkdf2;
        private readonly int keyBytes;
        private readonly IUserRepository userRepository;

        public PrimitiveSecretsService(IConfiguration configuration, IUserRepository userRepository)
        {
            iterationPbkdf2 = int.Parse(configuration["Safety:Iterations"]);
            keyBytes = int.Parse(configuration["Safety:KeyBytes"]);
            this.userRepository = userRepository;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetNonZeroBytes(salt);
            return salt;
        }

        public byte[] HashMasterPassword(string masterPassword, byte[] salt)
        {
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: masterPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterationPbkdf2,
                numBytesRequested: keyBytes
                );

            return hashed;
        }

        public byte[] HashUserPassword(string passwordPlainText, byte[] salt)
        {
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: passwordPlainText,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterationPbkdf2,
                numBytesRequested: keyBytes
                );

            return hashed;
        }

        public async Task<UserGetDTO> VerifyCredentials(string login, string passwordPlainText)
        {
            byte[] salt = await userRepository.GetSaltByLogin(login);
            if(salt == null)
            {
                return null;
            }

            byte[] password = HashUserPassword(passwordPlainText, salt);
            User foundUser = await userRepository.VerifyUserWithCredentials(login, password);
            if(foundUser == null)
            {
                return null;
            }

            return UserStaticMapper.GetDTOFromUser(foundUser);
        }

        public async Task<bool> VerifyMasterPassword(long id, string masterPasswordPlainText)
        {
            byte[] salt = await userRepository.GetSaltById(id);
            byte[] password = HashMasterPassword(masterPasswordPlainText, salt);
            bool verified = await userRepository.VerifyMasterPassword(id, password);
            return verified;
        }

        public Tuple<byte[], byte[]> CipherServicePassword(string servicePassword, string masterKey)
        {
            byte[] key = HashMasterPassword(masterKey, new byte[] { });
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.GenerateIV();

            byte[] iv = aes.IV;
            byte[] encrypted;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            { 
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {   
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(servicePassword);
                    encrypted = ms.ToArray();
                }
            }

            return new Tuple<byte[], byte[]>(encrypted, iv);
        }

        public string DecipherServicePassword(byte[] servicePassword, string masterKey, byte[] iv)
        {
            byte[] key = HashMasterPassword(masterKey, new byte[] { });
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.IV = iv;

            string decrypted;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(servicePassword))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {    
                    using (StreamReader reader = new StreamReader(cs))
                        decrypted = reader.ReadToEnd();
                }
            }

            return decrypted;
        }
    }
}
