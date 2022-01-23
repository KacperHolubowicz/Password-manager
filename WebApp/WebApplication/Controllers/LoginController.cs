using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.DTO.Blocking;
using Services.DTO.Device;
using Services.DTO.LoginAttempt;
using Services.DTO.User;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;
using WebApplication.ViewModels.User;
using IDeviceService = Services.Infrastructure.IDeviceService;

namespace WebApplication.Controllers
{
    //TODO refaktoryzacja, przeniesc wiekszosc serwisow np do authservice
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly ILoginAttemptService loginAttemptService;
        private readonly IDetectionService detectionService;
        private readonly IAuthService authService;
        private readonly IBlockingService blockingService;
        private readonly IDeviceService deviceService;

        public LoginController(IConfiguration configuration, IUserService userService, 
            ILoginAttemptService loginAttemptService, IDetectionService detectionService,
            IAuthService authService, IBlockingService blockingService,
            IDeviceService deviceService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.loginAttemptService = loginAttemptService;
            this.detectionService = detectionService;
            this.authService = authService;
            this.blockingService = blockingService;
            this.deviceService = deviceService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //todo wyczyscic ten chlew
        //todo wyswietlac ile zostalo blokady? optional
        [HttpPost]
        public async Task<IActionResult> Index(UserLoginVM userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string remoteIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
            await Delay(3000);

            bool isBlocked = await blockingService.CheckIfCurrentlyBlocked(remoteIp);
            if(isBlocked)
            {
                ViewData["LoginError"] = "You've still got time block for logging in.";
                return View();
            }

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
                return View();
            }
            else
            {
                string deviceType = detectionService.Device.Type.ToString();
                string browser = detectionService.Browser.Name.ToString();
                string system = detectionService.Platform.Name.ToString();
                DevicePostDTO device = new DevicePostDTO()
                {
                    IpAddress = remoteIp,
                    Browser = browser,
                    DeviceType = deviceType,
                    OperatingSystem = system
                };

                bool isKnown = await deviceService.VerifyDeviceAsync(user.ID, device);
                if(!isKnown)
                {
                    await deviceService.SaveDeviceAsync(user.ID, device);
                    TempData["DeviceMessage"] = "A new device has been detected and saved!";
                }
                await HttpContext.SignInAsync(authService.CreateClaims(user));
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        //todo komunikat przy nieunikalnym loginie/username/emailu
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
        [ActionName("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task Delay(int duration)
        {
            await Task.Delay(duration);
        }
    }
}
