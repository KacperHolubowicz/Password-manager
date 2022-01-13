using Repository.Infrastructure;
using Services.DTO.LoginAttempt;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class LoginAttemptService : ILoginAttemptService
    {
        private readonly ILoginAttemptRepository attemptRepository;

        public LoginAttemptService(ILoginAttemptRepository attemptRepository)
        {
            this.attemptRepository = attemptRepository;
        }

        public async Task<int> GetFrequentAttemptCountAsync(string ipAddress)
        {
            int attemptCount = await attemptRepository.GetFrequentAttemptCountAsync(ipAddress);
            return attemptCount;
        }

        public async Task SaveAttemptsAsync(LoginAttemptPostDTO attempt)
        {
            await attemptRepository.SaveAttemptsAsync(
                LoginAttemptStaticMapper.GetAttemptFromDTO(attempt)
                );
        }
    }
}
