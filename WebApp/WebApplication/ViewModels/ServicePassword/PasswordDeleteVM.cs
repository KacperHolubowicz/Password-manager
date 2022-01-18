using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.ServicePassword
{
    public class PasswordDeleteVM
    {
        public long ID { get; set; }

        [Display(Name = "Service name or URL")]
        public string Description { get; set; }
        [Required(ErrorMessage = "You need to confirm with master password")]
        [Display(Name = "Confirm with your master password")]
        public string MasterPassword { get; set; }
    }
}
