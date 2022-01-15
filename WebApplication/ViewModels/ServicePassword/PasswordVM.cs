using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.ServicePassword
{
    public class PasswordVM
    {
        public long ID { get; set; }

        [Display(Name = "Service name or URL")]
        public string Description { get; set; }
    }
}
