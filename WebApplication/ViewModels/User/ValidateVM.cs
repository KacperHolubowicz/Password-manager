using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.User
{
    public class ValidateVM
    {
        [Required]
        public string MasterPassword { get; set; }
    }
}
