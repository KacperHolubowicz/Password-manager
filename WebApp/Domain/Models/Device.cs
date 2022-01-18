namespace Domain.Models
{
    /// <summary>
    /// Database entity class used to differentiate devices used to log in to a particular user account
    /// </summary>
    public class Device
    {
        public long ID { get; set; }
        public string Browser { get; set; }
        public string DeviceType { get; set; }
        public string OperatingSystem { get; set; }
        public string IpAddress { get; set; }
    }
}