using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository.Exceptions;
using Services.DTO.Device;
using Services.DTO.User;
using Services.Infrastructure;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;
using WebApplication.ViewModels.User;
using IDeviceService = Services.Infrastructure.IDeviceService;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
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
            this.detectionService = detectionService;
            this.authService = authService;
            this.blockingService = blockingService;
            this.deviceService = deviceService;
        }

        public IActionResult Index()
        {
            return View();
        }

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
            if (isBlocked)
            {
                ViewData["LoginError"] = "You've still got time block for logging in.";
                return View();
            }


            UserGetDTO user = await authService.TryLoginUser(userLogin.Login, userLogin.Password, remoteIp);
            if (user == null)
            {
                string errorMessage = await authService.CheckUserBlock(remoteIp);
                ViewData["LoginError"] = errorMessage;
                return View();
            }

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
            if (!isKnown)
            {
                await deviceService.SaveDeviceAsync(user.ID, device);
                TempData["DeviceMessage"] = "A new device has been detected and saved!";
            }
            await HttpContext.SignInAsync(authService.CreateClaims(user));
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM userRegister)
        {
            if (!ModelState.IsValid)
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
            try
            {
                await userService.AddUserAsync(post);
            }
            catch (NonUniqueException ex)
            {
                TempData["NonUnique"] = ex.Message;
                return View();
            }

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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string email = forgotPassword.Email;
            bool doesEmailExist = await userService.FindUserWithEmail(email);

            if (!doesEmailExist)
            {
                TempData["Email"] = "There is no account connected with provided email";
                return View();
            }

            TempData["Email"] = $"Link to password reset SHOULD HAVE BEEN sent to {email}";
            return RedirectToAction("Index");
        }

        private async Task Delay(int duration)
        {
            await Task.Delay(duration);
        }
    }
}
