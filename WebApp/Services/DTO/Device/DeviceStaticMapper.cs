namespace Services.DTO.Device
{
    public class DeviceStaticMapper
    {
        public static Domain.Models.Device GetDeviceFromDTO(DevicePostDTO devicePost)
        {
            return new Domain.Models.Device()
            {
                DeviceType = devicePost.DeviceType,
                Browser = devicePost.Browser,
                IpAddress = devicePost.IpAddress,
                OperatingSystem = devicePost.OperatingSystem
            };
        }

        public static DeviceGetDTO GetDTOFromDevice(Domain.Models.Device device)
        {
            return new DeviceGetDTO()
            {
                ID = device.ID,
                Browser = device.Browser,
                DeviceType = device.DeviceType,
                OperatingSystem = device.OperatingSystem
            };
        }
    }
}
