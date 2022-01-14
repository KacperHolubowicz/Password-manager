using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.ViewModels.User;

namespace WebApplication.Views.Login
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserRegisterVM UserRegisterVM { get; set; }

        public void OnGet()
        {
        }
    }
}
