using Services.DTO.ServicePassword;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    /// <summary>
    /// Service layer class for all backend logic connected with service passwords
    /// </summary>
    public interface IServicePasswordService
    {
        public Task<List<ServicePasswordGetDTO>> FindAllPasswordsAsync(long userId);
        public Task<ServicePasswordGetDTO> FindPasswordByIdAsync(long ownerId, long passwordId);
        public Task CreatePasswordAsync(long ownerId, ServicePasswordPostDTO password, string masterKey);
        public Task UpdatePasswordAsync(ServicePasswordPutDTO password, long ownerId, long passwordId, string masterKey);
        public Task DeletePasswordAsync(long ownerId, long passwordId);
        public Task DeleteUserWithPasswordsAsync(long userId);
    }
}
