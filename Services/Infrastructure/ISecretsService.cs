using Services.DTO.User;
using System;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface ISecretsService
    {
        public byte[] GenerateSalt();
        public byte[] HashUserPassword(string passwordPlainText, byte[] salt);
        public byte[] HashMasterPassword(string masterPassword, byte[] salt);
        public Task<UserGetDTO> VerifyCredentials(string login, string passwordPlainText);
        public Task<bool> VerifyMasterPassword(long id, string login, string masterPasswordPlainText);
        public Tuple<byte[],byte[]> CipherServicePassword(string servicePassword, string masterKey);
        public string DecipherServicePassword(byte[] servicePassword, string masterKey, byte[] iv);

    }
}
