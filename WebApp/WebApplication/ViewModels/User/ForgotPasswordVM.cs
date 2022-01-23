using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.User 
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "You need to provide email which you used for your registration")]
        [EmailAddress(ErrorMessage = "Provided input is not an email")]
        public string Email { get; set; }
    }
}