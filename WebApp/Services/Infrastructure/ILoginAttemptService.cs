using Services.DTO.LoginAttempt;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    /// <summary>
    /// Service layer class for all backend logic connected with all login attempts,
    /// whether successful or not
    /// </summary>
    public interface ILoginAttemptService
    {
        public Task<int> GetFrequentAttemptCountAsync(string ipAddress);
        public Task SaveAttemptsAsync(LoginAttemptPostDTO attempt);
    }
}
