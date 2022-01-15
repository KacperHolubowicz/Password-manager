using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.User
{
    //TODO uporzadkowac klasy z view models
    public class PasswordCreateVM
    {
        [Required]
        [Display(Name = "Service name or URL")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Password for that service")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm with your master password")]
        public string MasterPassword { get; set; }
    }
}
