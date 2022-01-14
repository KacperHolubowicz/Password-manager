using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.DTO.Blocking;
using Services.DTO.LoginAttempt;
using Services.DTO.User;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.User;

namespace WebApplication.Controllers
{
    //TODO refaktoryzacja, przeniesc wiekszosc serwisow np do authservice
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly ILoginAttemptService loginAttemptService;
        private readonly IAuthService authService;
        private readonly IBlockingService blockingService;

        public LoginController(IConfiguration configuration, IUserService userService, 
            ILoginAttemptService loginAttemptService,
            IAuthService authService, IBlockingService blockingService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.loginAttemptService = loginAttemptService;
            this.authService = authService;
            this.blockingService = blockingService;
        }

        public IActionResult Index()
        {
            return View();
        }
        //TODO sprawdzanie blokady przed logowaniem, kontrola device
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string remoteIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            UserGetDTO user = await userService.LoginUser(userLogin.Login, userLogin.Password);
            await loginAttemptService.SaveAttemptsAsync(new LoginAttemptPostDTO()
            {
                IpAddress = remoteIp,
                Successful = !(user == null),
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

            if (user == null)
            {
                string errorMessage;
                int count = await loginAttemptService.GetFrequentAttemptCountAsync(remoteIp);

                if (count % 5 == 0)
                {
                    int blocks = await blockingService.GetBlockCountAsync(remoteIp);
                    int baseBlock = int.Parse(configuration["Safety:BaseBlockInMinutes"]);
                    int incrementBlock = int.Parse(configuration["Safety:IncrementBlockInMinutes"]);
                    int blockTime = baseBlock + incrementBlock * blocks;

                    errorMessage = $"You exceeded max attempts count. You are blocked for {blockTime} minutes.";

                    await blockingService.SaveBlocking(new BlockingPostDTO()
                    {
                        IpAddress = remoteIp,
                        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        BlockedUntil = DateTime.Now.AddMinutes(blockTime).ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
                else
                {
                    int attempts = int.Parse(configuration["Safety:MaxAttempts"]) - count;
                    errorMessage = $"Incorrent login or password. You have {attempts} " +
                    $"attempts to log in.";
                }

                ViewData["LoginError"] = errorMessage;
                return View("Index");
            }
            else
            {
                await HttpContext.SignInAsync(authService.CreateClaims(user));
                return Redirect("../");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM userRegister)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            UserPostDTO post = new UserPostDTO()
            {
                Login = userRegister.Login,
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = userRegister.Password,
                MasterPassword = userRegister.MasterPassword
            };

            await userService.AddUserAsync(post);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("Index", "Home");
        }
    }
}
