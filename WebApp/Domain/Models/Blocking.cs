namespace Domain.Models
{
    /// <summary>
    /// Database entity class for saving all blockades for various ip addresses
    /// </summary>
    public class Blocking
    {
        public long ID { get; set; }
        public string IpAddress { get; set; }
        public string Timestamp { get; set; }
        public string BlockedUntil { get; set; }
    }
}
