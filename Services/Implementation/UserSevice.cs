using Domain.Models;
using Repository.Infrastructure;
using Services.DTO.User;
using Services.Infrastructure;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class UserSevice : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserSevice(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUserAsync(UserPostDTO user)
        {
            await userRepository.AddUserAsync(UserStaticMapper.GetUserFromDTO(user));
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
    }
}
