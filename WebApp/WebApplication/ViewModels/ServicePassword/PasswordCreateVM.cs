using System.ComponentModel.DataAnnotations;
using WebApplication.ViewModels.Attributes;

namespace WebApplication.ViewModels.ServicePassword
{
    public class PasswordCreateVM
    {
        [Required]
        [Display(Name = "Service name or URL")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Password for that service")]
        [StrongPassword(8, 28)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm with your master password")]
        public string MasterPassword { get; set; }
    }
}
