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
            if (!ModelState.IsValid)
            {
                return View();
            }

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
        //TODO komunikaty/wyjatki zeby zwrocic np 403 przy dobieraniu sie do cudzych hasel

        public async Task<IActionResult> Edit(long id)
        {
            long userId = GetUserID();
            ServicePasswordGetDTO passwordGetDTO = await passwordService.FindPasswordByIdAsync(userId, id);
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
            if (!ModelState.IsValid)
            {
                return View();
            }

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

            await passwordService.UpdatePasswordAsync(passwordPut, userId, passwordEdit.ID, providedMasterPass);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(long id)
        {
            long userId = GetUserID();

            ServicePasswordGetDTO passwordGetDTO = await passwordService.FindPasswordByIdAsync(userId, id);
            PasswordShowVM passwordVM = new PasswordShowVM()
            {
                ID = id,
                Description = passwordGetDTO.Description,
                Password = "Provide master password to show password",
                MasterPassword = ""
            };

            return View(passwordVM);
        }

        //TODO sprawdzic czy da sie inaczej naprawic problem z nullami w argumencie
        [HttpPost]
        public async Task<IActionResult> Show(PasswordShowVM passwordShowVM)
        {
            long userId = GetUserID();
            string masterPassword = passwordShowVM.MasterPassword;
            ServicePasswordGetDTO passwordVal = await passwordService.FindPasswordByIdAsync
                (userId, passwordShowVM.ID);
            passwordShowVM.MasterPassword = "";
            passwordShowVM.Description = passwordVal.Description;
            passwordShowVM.Password = "Provide master password to show password";

            if (string.IsNullOrWhiteSpace(masterPassword))
            {
                return View(passwordShowVM);
            }
            
            bool verified = await secretsService.VerifyMasterPassword(userId, masterPassword);
            if(!verified)
            {
                ViewData["MasterError"] = "Invalid master password";
                return View(passwordShowVM);
            }

            string decryptedPassword = secretsService.DecipherServicePassword(passwordVal.Password,
                masterPassword, passwordVal.IV);
            passwordShowVM.Password = decryptedPassword;
            

            return View(passwordShowVM);
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
