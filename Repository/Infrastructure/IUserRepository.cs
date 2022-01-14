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
        public Task<User> VerifyUserWithCredentials(string login, byte[] userPassword);
        public Task<bool> VerifyMasterPassword(long userId, byte[] masterPassword);
        public Task<byte[]> GetSaltByLogin(string login);
    }
}
