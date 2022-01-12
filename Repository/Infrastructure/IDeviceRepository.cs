using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Infrastructure
{
    /// <summary>
    /// Repository layer for database operation for devices saved by user
    /// </summary>
    public interface IDeviceRepository
    {
        public Task<List<Device>> FindAllDevicesAsync(long userId);
        public Task SaveDeviceAsync(Device device);
        public Task<bool> VerifyDeviceAsync(Device device);
        public Task DeleteUserWithDevices(long userId);
    }
}
