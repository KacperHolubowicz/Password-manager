using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.User
{
    //TODO
    public class UserRegisterVM
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username requires at least 4 characters")]
        public string Username { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Username requires at least 4 characters")]
        public string Login { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string MasterPassword { get; set; }
        [Required]
        public string ConfirmMasterPassword { get; set; }
    }
}
