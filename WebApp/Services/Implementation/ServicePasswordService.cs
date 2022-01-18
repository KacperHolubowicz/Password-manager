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

        public async Task DeletePasswordAsync(long ownerId, long passwordId)
        {
            await passwordRepository.DeletePasswordAsync(ownerId, passwordId);
        }

        public async Task DeleteUserWithPasswordsAsync(long userId)
        {
            await passwordRepository.DeleteUserWithPasswordsAsync(userId);
        }

        public async Task<List<ServicePasswordGetDTO>> FindAllPasswordsAsync(long userId)
        {
            List<ServicePassword> passwords = await passwordRepository.FindAllPasswordsAsync(userId);
            return passwords
                .Select(pass => ServicePasswordStaticMapper.GetDTOFromPassword(pass))
                .ToList();
        }

        public async Task<ServicePasswordGetDTO> FindPasswordByIdAsync(long ownerId, long passwordId)
        {
            ServicePassword password = await passwordRepository.FindPasswordById(ownerId, passwordId);
            return ServicePasswordStaticMapper.GetDTOFromPassword(password);
        }

        public async Task UpdatePasswordAsync(ServicePasswordPutDTO password, long ownerId, long passwordId, string masterKey)
        {
            await passwordRepository.UpdatePasswordAsync(
                ServicePasswordStaticMapper.GetPasswordFromDTO(password, secretsService, masterKey), 
                ownerId,
                passwordId
                );
        }
    }
}
