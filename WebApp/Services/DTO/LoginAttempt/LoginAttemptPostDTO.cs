namespace Services.DTO.LoginAttempt
{
    public class LoginAttemptPostDTO
    {
        public string IpAddress { get; set; }
        public string Timestamp { get; set; }
        public bool Successful { get; set; }
    }
}
