using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.ViewModels.Device
{
    public class DeviceVM
    {
        public long ID { get; set; }
        public string Browser { get; set; }
        [Display(Name = "Device type")]
        public string DeviceType { get; set; }
        [Display(Name = "Operating system")]
        public string OperatingSystem { get; set; }
    }
}
