﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.Attributes;

namespace WebApplication.ViewModels.ServicePassword
{
    public class PasswordEditVM
    {
        public long ID { get; set; }

        [Required]
        [Display(Name = "Service name or URL")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "New password for that service")]
        [StrongPassword(8, 28)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm with your master password")]
        public string MasterPassword { get; set; }
    }
}
