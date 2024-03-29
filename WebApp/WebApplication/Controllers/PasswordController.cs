﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
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

        public async Task<IActionResult> Edit(long id)
        {
            long userId = GetUserID();
            ServicePasswordGetDTO passwordGetDTO;

            try {
                passwordGetDTO = await passwordService.FindPasswordByIdAsync(userId, id);
            } catch(UnauthorizedResourceException)
            {
                return Forbid();
            }

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
            ServicePasswordGetDTO passwordGetDTO;
            
            try {
                passwordGetDTO = await passwordService.FindPasswordByIdAsync(userId, id);
            } catch(UnauthorizedResourceException)
            {
                return Forbid();
            }

            PasswordShowVM passwordVM = new PasswordShowVM()
            {
                ID = id,
                Description = passwordGetDTO.Description,
                Password = "Provide master password to show password",
                MasterPassword = ""
            };

            return View(passwordVM);
        }

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

        public async Task<IActionResult> Delete(long id)
        {
            long userId = GetUserID();
            ServicePasswordGetDTO passwordGetDTO;
            
            try {
                passwordGetDTO = await passwordService.FindPasswordByIdAsync(userId, id);
            } catch(UnauthorizedResourceException)
            {
                return Forbid();
            }

            PasswordDeleteVM passwordVM = new PasswordDeleteVM()
            {
                ID = id,
                Description = passwordGetDTO.Description,
                MasterPassword = ""
            };

            return View(passwordVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PasswordDeleteVM passwordToDelete)
        {
            long userId = GetUserID();

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool verified = await secretsService.VerifyMasterPassword(userId, passwordToDelete.MasterPassword);
            if (!verified)
            {
                ViewData["MasterError"] = "Invalid master password";
                return View();
            }

            await passwordService.DeletePasswordAsync(userId, passwordToDelete.ID);
            return RedirectToAction(nameof(Index));
        }

        private long GetUserID()
        {
            long userId = int.Parse(User.Claims.First(c => c.Type == "ID").Value);
            if(userId == 0)
            {
                throw new UnauthorizedAccessException();
            }
            return userId;
        }
    }
}
