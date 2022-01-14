namespace Services.DTO.LoginAttempt
{
    public class LoginAttemptStaticMapper
    {
        public static Domain.Models.LoginAttempt GetAttemptFromDTO
            (LoginAttemptPostDTO attemptPost)
        {
            return new Domain.Models.LoginAttempt()
            {
                IpAddress = attemptPost.IpAddress,
                Successful = attemptPost.Successful,
                Timestamp = attemptPost.Timestamp
            };
        }
    }
}
