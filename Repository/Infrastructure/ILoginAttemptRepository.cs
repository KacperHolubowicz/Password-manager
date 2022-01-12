using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Infrastructure
{
    /// <summary>
    /// Repository layer interface for saving all login attempts coming from particular Ip address
    /// </summary>
    public interface ILoginAttemptRepository
    {
        public Task<int> GetFrequentAttemptCountAsync(string ipAddress);
        public Task SaveAttemptsAsync(LoginAttempt attempt);
    }
}
