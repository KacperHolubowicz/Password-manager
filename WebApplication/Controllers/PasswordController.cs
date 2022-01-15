using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.ServicePassword;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.ServicePassword;

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
                Description = p.Description
            }).ToList();

            return View(passwordsVM);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PasswordCreateVM passwordCreate)
        {

            ServicePasswordPostDTO passwordPost = new ServicePasswordPostDTO()
            {
                Description = passwordCreate.Description,
                Password = passwordCreate.Password
            };

            long userId = GetUserID();
            string providedMasterPass = passwordCreate.MasterPassword;

            bool verified = await secretsService.VerifyMasterPassword(userId, providedMasterPass);
            if(!verified)
            {
                ViewData["MasterError"] = "Invalid master password";
                return View();
            }

            await passwordService.CreatePasswordAsync(userId, passwordPost, providedMasterPass);
            return RedirectToAction(nameof(Index));
        }
        //TODO edit show delete wraz z pytaniem o master passworda - weryfikacja ilosci podan? nie wiadomo 
        //TODO BARDZO WAZNE upewnic sie ze nie ma accessu do cudzych hasel

        public async Task<IActionResult> Edit(long id)
        {
            ServicePasswordGetDTO passwordGetDTO = await passwordService.FindPasswordByIdAsync(id);
            PasswordEditVM passwordVM = new PasswordEditVM()
            {
                ID = id,
                Description = passwordGetDTO.Description,
                Password = "",
                MasterPassword = ""
            };

            return View(passwordVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PasswordEditVM passwordEdit)
        {
            ServicePasswordPutDTO passwordPut = new ServicePasswordPutDTO()
            {
                Description = passwordEdit.Description,
                Password = passwordEdit.Password
            };

            long userId = GetUserID();
            string providedMasterPass = passwordEdit.MasterPassword;

            bool verified = await secretsService.VerifyMasterPassword(userId, providedMasterPass);
            if (!verified)
            {
                ViewData["MasterError"] = "Invalid master password";
                return View();
            }

            await passwordService.UpdatePasswordAsync(passwordPut, passwordEdit.ID, providedMasterPass);
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
