using Domain.Models;
using Repository.Infrastructure;
using Services.DTO.User;
using Services.Infrastructure;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ISecretsService secretsService;

        public UserService(IUserRepository userRepository, ISecretsService secretsService)
        {
            this.userRepository = userRepository;
            this.secretsService = secretsService;
        }

        public async Task AddUserAsync(UserPostDTO user)
        {
            User userToAdd = UserStaticMapper.GetUserFromDTO(user);
            userToAdd.Salt = secretsService.GenerateSalt();
            userToAdd.Password = secretsService.HashUserPassword(user.Password, userToAdd.Salt);
            userToAdd.MasterPassword = secretsService.HashMasterPassword(user.MasterPassword, userToAdd.Salt);
            await userRepository.AddUserAsync(userToAdd);
        }

        public async Task DeleteUserAsync(long userId)
        {
            await userRepository.DeleteUserAsync(userId);
        }

        public async Task<UserGetDTO> FindUserAsync(long userId)
        {
            User foundUser = await userRepository.FindUserAsync(userId);
            return UserStaticMapper.GetDTOFromUser(foundUser);
        }

        public async Task<UserGetDTO> LoginUser(string login, string passwordPlainText)
        {
            var user = await secretsService.VerifyCredentials(login, passwordPlainText);
            return user;
        }
    }
}
