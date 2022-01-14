namespace Services.DTO.ServicePassword
{
    public class ServicePasswordGetDTO
    {
        public long ID { get; set; }
        public string Description { get; set; }
        public byte[] Password { get; set; }
        public byte[] IV { get; set; }
    }
}
