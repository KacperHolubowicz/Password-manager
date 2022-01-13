using Services.DTO.User;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    /// <summary>
    /// Service layer class for all backend logic connected with users
    /// </summary>
    public interface IUserService
    {
        public Task<UserGetDTO> FindUserAsync(long userId);
        public Task AddUserAsync(UserPostDTO user);
        public Task DeleteUserAsync(long userId);
    }
}
