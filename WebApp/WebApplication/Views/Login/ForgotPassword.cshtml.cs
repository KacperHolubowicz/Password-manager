using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.ViewModels.User;

namespace WebApplication.Views.Login
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty] public ForgotPasswordVM ForgotPassword { get; set; }
        public void OnGet()
        {
        }
    }
}