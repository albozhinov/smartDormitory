using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Models;
using smartDormitory.WEB.Providers;

namespace smartDormitory.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IICBApiSensorsService apiSensorsService;
        private readonly IUserSensorService userSensorService;
        private readonly IUserManager<User> _userManager;
        private readonly IMemoryCache _cache;

        public HomeController(IICBApiSensorsService apiSensorsService, IUserSensorService userSensorService, IUserManager<User> userManager, IMemoryCache cache)
        {
            this.apiSensorsService = apiSensorsService ?? throw new ArgumentNullException(nameof(apiSensorsService));
            this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IActionResult> Index()
        {
            var allPublicSensors = await this.GetAllPublicUserSensorsCashedAsync();
            List<UserSensors> allSensors = null;

            if (this.User.Identity.IsAuthenticated)
            {
                string userId = await this._userManager.GetUserIdAsync(await this._userManager.GetUserAsync(HttpContext.User));
                var userSensors = await this.GetAllPrivateUserSensorsCashedAsync(userId);
                allSensors = allPublicSensors.ToList();
                allSensors.AddRange(userSensors);
            }
            else
            {
                allSensors = allPublicSensors.ToList();
            }

            var sensorsViewModel = allSensors
                                    .Select(s => new UserSensorViewModel()
                                    {
                                        Latitude = s.Latitude,
                                        Longtitude = s.Longitude,
                                        UserName = s.User.UserName,
                                        Description = s.Description,
                                        Tag = s.Sensor.Tag,
                                        Id = s.Id,
                                        ModifiedOn = s.Sensor.ModifiedOn,
                                        Value = s.Sensor.Value,
                                        Alarm = s.Alarm,
                                        Name = s.Name,
                                        URL = s.Sensor.Url,
                                    })
                                    .ToList();

            return View(sensorsViewModel);
        }

        private async Task<IEnumerable<UserSensors>> GetAllPublicUserSensorsCashedAsync()
        {
            return await this._cache.GetOrCreate("AllPublicUserSensors", entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddHours(3);
                return this.userSensorService.GetAllPublicUsersSensorsAsync();
            });
        }

        private async Task<IEnumerable<UserSensors>> GetAllPrivateUserSensorsCashedAsync(string userId)
        {
            return await this._cache.GetOrCreate("AllPrivateUserSensors", entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddHours(3);
                return this.userSensorService.GetAllPrivateUserSensorsAsync(userId);
            });
        }
    }
}
