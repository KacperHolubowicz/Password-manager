namespace Domain.Models
{
    /// <summary>
    /// Database entity class for saving all login attempts from a specific public IP address
    /// </summary>
    public class LoginAttempt
    {
        public long ID { get; set; }
        public string IpAddress { get; set; }
        public string Timestamp { get; set; }
        public bool Successful { get; set; }
    }
}
