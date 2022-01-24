using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Repository.Exceptions;
using Services.DTO.Blocking;
using Services.DTO.LoginAttempt;
using Services.DTO.User;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly ILoginAttemptService loginAttemptService;
        private readonly IBlockingService blockingService;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthService(ILoginAttemptService loginAttemptService,
            IBlockingService blockingService, IConfiguration configuration,
            IUserService userService)
        {
            this.loginAttemptService = loginAttemptService;
            this.blockingService = blockingService;
            this.configuration = configuration;
            this.userService = userService;
        }

        public ClaimsPrincipal CreateClaims(UserGetDTO user)
        {
            Claim[] claims = new Claim[3]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim("ID", user.ID.ToString())
            };

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                );
        }

        public async Task<UserGetDTO> TryLoginUser(string login, string password, string remoteIp)
        {
            bool couldLogin = true;
            UserGetDTO user = null;

            try
            {
                user = await userService.LoginUser(login, password);
            }
            catch (InvalidCredentialsException)
            {
                couldLogin = false;
            }

            await loginAttemptService.SaveAttemptsAsync(new LoginAttemptPostDTO()
            {
                IpAddress = remoteIp,
                Successful = couldLogin,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

            return user;
        }

        public async Task<string> CheckUserBlock(string remoteIp)
        {
            string errorMessage;
            int count = await loginAttemptService.GetFrequentAttemptCountAsync(remoteIp);

            if (count % 5 == 0)
            {
                int blockTime = await BlockUser(remoteIp);
                errorMessage = $"You exceeded max attempts count. You are blocked for {blockTime} minutes.";
            }
            else
            {
                int attempts = int.Parse(configuration["Safety:MaxAttempts"]) - (count % 5);
                errorMessage = $"Incorrent login or password. You have {attempts} " +
                $"attempts to log in.";
            }

            return errorMessage;
        }

        private async Task<int> BlockUser(string remoteIp)
        {
            int blocks = await blockingService.GetBlockCountAsync(remoteIp);
            int baseBlock = int.Parse(configuration["Safety:BaseBlockInMinutes"]);
            int incrementBlock = int.Parse(configuration["Safety:IncrementBlockInMinutes"]);
            int blockTime = baseBlock + incrementBlock * blocks;

            await blockingService.SaveBlocking(new BlockingPostDTO()
            {
                IpAddress = remoteIp,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                BlockedUntil = DateTime.Now.AddMinutes(blockTime).ToString("yyyy-MM-dd HH:mm:ss")
            });

            return blockTime;
        }
    }
}
