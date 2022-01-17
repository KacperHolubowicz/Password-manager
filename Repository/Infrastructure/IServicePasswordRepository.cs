using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Infrastructure
{
    /// <summary>
    /// Repository layer for database operation for service passwords provided by users
    /// </summary>
    public interface IServicePasswordRepository
    {
        public Task<List<ServicePassword>> FindAllPasswordsAsync(long userId);
        public Task<ServicePassword> FindPasswordById(long ownerId, long passwordId);
        public Task CreatePasswordAsync(long ownerId, ServicePassword password);
        public Task UpdatePasswordAsync(ServicePassword password, long ownerId, long passwordId);
        public Task DeletePasswordAsync(long ownerId, long passwordId);
        public Task DeleteUserWithPasswordsAsync(long userId);
    }
}
