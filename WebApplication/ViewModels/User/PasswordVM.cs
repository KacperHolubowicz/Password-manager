using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.User
{
    public class PasswordVM
    {
        public long ID { get; set; }

        [Display(Name = "Service name or URL")]
        public string Description { get; set; }

        [Display(Name = "Encrypted password")]
        public string EncryptedPassword { get; set; }
        public byte[] IV { get; set; }
    }
}
