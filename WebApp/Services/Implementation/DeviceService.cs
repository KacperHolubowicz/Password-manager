using Domain.Models;
using Repository.Infrastructure;
using Services.DTO.Device;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public async Task DeleteUserWithDevices(long userId)
        {
            await deviceRepository.DeleteUserWithDevices(userId);
        }

        public async Task<List<DeviceGetDTO>> FindAllDevicesAsync(long userId)
        {
            List<Device> devices = await deviceRepository.FindAllDevicesAsync(userId);
            return devices
                .Select(dev => DeviceStaticMapper.GetDTOFromDevice(dev))
                .ToList();
        }

        public async Task SaveDeviceAsync(long userId, DevicePostDTO device)
        {
            await deviceRepository.SaveDeviceAsync(
                userId, DeviceStaticMapper.GetDeviceFromDTO(device)
                );
        }

        public async Task<bool> VerifyDeviceAsync(long ownerId, DevicePostDTO device)
        {
            bool isVerified = await deviceRepository.VerifyDeviceAsync(
                ownerId, DeviceStaticMapper.GetDeviceFromDTO(device)
                );
            return isVerified;
        }
    }
}
