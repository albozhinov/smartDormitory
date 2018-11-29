using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Models;

namespace smartDormitory.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly IICBApiSensorsService apiSensorsService;
        private readonly IUserSensorService userSensorService;

        public HomeController(IICBApiSensorsService apiSensorsService, IUserSensorService userSensorService)
        {
            this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
            this.apiSensorsService = apiSensorsService ?? throw new ArgumentNullException(nameof(apiSensorsService));
        }

        public async Task<IActionResult> Index()
        {
            var allPublicSensors = await this.userSensorService.GetAllPublicUsersSensors();

            var sensorsViewModel = allPublicSensors
                                    .Select(s => new UserSensorViewModel()
                                    {
                                        Latitude = s.Latitude,
                                        Longtitude = s.Longitude,
                                    })
                                    .ToList();

            return View(sensorsViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
