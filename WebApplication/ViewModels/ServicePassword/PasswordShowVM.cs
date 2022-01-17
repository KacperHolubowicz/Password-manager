using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.ServicePassword
{
    public class PasswordShowVM
    {
        public long ID { get; set; }

        [Required]
        [Display(Name = "Service name or URL")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Encrypt service's password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Master password is required for decryption")]
        [Display(Name = "Confirm with your master password")]
        public string MasterPassword { get; set; }
    }
}
