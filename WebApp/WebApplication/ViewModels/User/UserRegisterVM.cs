using System.ComponentModel.DataAnnotations;
using WebApplication.ViewModels.Attributes;

namespace WebApplication.ViewModels.User
{
    public class UserRegisterVM
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username requires at least 4 characters")]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        public string Username { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Login requires at least 6 characters")]
        [MaxLength(20, ErrorMessage = "Login cannot be longer than 20 characters")]
        public string Login { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StrongPassword(8, 26)]
        public string Password { get; set; }
        [Required]
        [PasswordConfirmation]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StrongPassword(8, 40)]
        [Display(Name = "Master password")]
        public string MasterPassword { get; set; }
        [Required]
        [MasterPasswordConfirmation]
        [Display(Name = "Confirm master password")]
        public string ConfirmMasterPassword { get; set; }
    }
}
