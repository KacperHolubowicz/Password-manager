using Domain.Models;
using Repository.Infrastructure;
using Services.DTO.ServicePassword;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ServicePasswordService : IServicePasswordService
    {
        private readonly IServicePasswordRepository passwordRepository;
        private readonly ISecretsService secretsService;

        public ServicePasswordService(IServicePasswordRepository passwordRepository
            , ISecretsService secretsService)
        {
            this.passwordRepository = passwordRepository;
            this.secretsService = secretsService;
        }

        public async Task CreatePasswordAsync(long ownerId, ServicePasswordPostDTO password, string masterKey)
        {
            await passwordRepository.CreatePasswordAsync(
                ownerId, 
                ServicePasswordStaticMapper.GetPasswordFromDTO(password, secretsService, masterKey)
                );
        }

        public async Task DeletePasswordAsync(long passwordId)
        {
            await passwordRepository.DeletePasswordAsync(passwordId);
        }

        public async Task DeleteUserWithPasswordsAsync(long userId)
        {
            await passwordRepository.DeletePasswordAsync(userId);
        }

        public async Task<List<ServicePasswordGetDTO>> FindAllPasswordsAsync(long userId)
        {
            List<ServicePassword> passwords = await passwordRepository.FindAllPasswordsAsync(userId);
            return passwords
                .Select(pass => ServicePasswordStaticMapper.GetDTOFromPassword(pass))
                .ToList();
        }

        public async Task UpdatePasswordAsync(ServicePasswordPutDTO password, long passwordId, string masterKey)
        {
            await passwordRepository.UpdatePasswordAsync(
                ServicePasswordStaticMapper.GetPasswordFromDTO(password, secretsService, masterKey), passwordId
                );
        }
    }
}
