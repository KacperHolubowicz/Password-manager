using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.ServicePassword;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.User;

namespace WebApplication.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class PasswordController : Controller
    {
        private readonly IServicePasswordService passwordService;
        private readonly ISecretsService secretsService;

        public PasswordController(IServicePasswordService passwordService,
            ISecretsService secretsService)
        {
            this.passwordService = passwordService;
            this.secretsService = secretsService;
        }

        public async Task<IActionResult> Index()
        {
            long userId = GetUserID();
            List <ServicePasswordGetDTO> passwords = await passwordService.FindAllPasswordsAsync(userId);
            List<PasswordVM> passwordsVM = passwords.Select(p => new PasswordVM()
            {
                ID = p.ID,
                Description = p.Description,
                EncryptedPassword = Convert.ToBase64String(p.Password),
                IV = p.IV
            }).ToList();

            return View(passwordsVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        //TODO zapytanie o master password
        [HttpPost]
        public async Task<IActionResult> Create(PasswordCreateVM passwordCreate)
        {
            ServicePasswordPostDTO passwordPost = new ServicePasswordPostDTO()
            {
                Description = passwordCreate.Description,
                Password = passwordCreate.Password
            };

            long userId = GetUserID();
            await passwordService.CreatePasswordAsync(userId, passwordPost, "LiXoWbbB");
            return RedirectToAction(nameof(Index));
        }

        //TODO lepsza obsluga przy bledach/zerowym id
        private long GetUserID()
        {
            long userId = int.Parse(User.Claims.First(c => c.Type == "ID").Value);
            if(userId == 0)
            {
                throw new Exception();
            }
            return userId;
        }
    }
}
