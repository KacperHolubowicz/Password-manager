namespace Services.DTO.Blocking
{
    public class BlockingPostDTO
    {
        public long ID { get; set; }
        public string IpAddress { get; set; }
        public string Timestamp { get; set; }
        public string BlockedUntil { get; set; }
    }
}
