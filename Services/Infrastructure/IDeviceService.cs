using Services.DTO.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface IDeviceService
    {
        /// <summary>
        /// Service layer class for all backend logic connected with devices used by users
        /// </summary>
        public Task<List<DeviceGetDTO>> FindAllDevicesAsync(long userId);
        public Task SaveDeviceAsync(long userId, DevicePostDTO device);
        public Task<bool> VerifyDeviceAsync(long ownerId, DevicePostDTO device);
        public Task DeleteUserWithDevices(long userId);
    }
}
