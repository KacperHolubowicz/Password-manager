using Domain.Models;
using System.Threading.Tasks;

namespace Repository.Infrastructure
{
    /// <summary>
    /// Repository layer for database operation for User entity
    /// </summary>
    public interface IUserRepository
    {
        public Task<User> FindUserAsync(long userId);
        public Task AddUserAsync(User user);
        public Task DeleteUserAsync(long userId);
    }
}
