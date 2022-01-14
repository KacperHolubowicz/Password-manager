using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.Attributes;

namespace WebApplication.ViewModels.User
{
    public class UserLoginVM
    {
        [Required]
        [MinLength(6, ErrorMessage = "Login requires at least 6 characters")]
        [MaxLength(20, ErrorMessage = "Login cannot be longer than 20 characters")]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
