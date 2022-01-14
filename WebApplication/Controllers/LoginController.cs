using Microsoft.AspNetCore.Mvc;
using Services.DTO.User;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.User;

namespace WebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM userLogin)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var test = await userService.LoginUser(userLogin.Login, userLogin.Password);
            if (test != null)
                return Content($"{test.Username}, {test.ID}, {test.Email}");
            else
                return Content("Wrong login or password");
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
    }
}
