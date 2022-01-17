using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.Device;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.ViewModels.Device;

namespace WebApplication.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DeviceController : Controller
    {
        private readonly IDeviceService deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        public async Task<IActionResult> Index()
        {
            List<DeviceGetDTO> devicesDTO = await deviceService
                .FindAllDevicesAsync(GetUserID());
            List<DeviceVM> devices = devicesDTO
                .Select(d => new DeviceVM()
                {
                    ID = d.ID,
                    Browser = d.Browser,
                    DeviceType = d.DeviceType,
                    OperatingSystem = d.OperatingSystem
                }).ToList();

            return View(devices);
        }

        private long GetUserID()
        {
            long userId = int.Parse(User.Claims.First(c => c.Type == "ID").Value);
            if (userId == 0)
            {
                throw new Exception();
            }
            return userId;
        }
    }
}
