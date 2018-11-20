using System;
using System.Diagnostics;
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

        public HomeController(IICBApiSensorsService apiSensorsService)
        {
            this.apiSensorsService = apiSensorsService ?? throw new ArgumentNullException(nameof(apiSensorsService));
        }

        public IActionResult Index()
        {
            return View();
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

        public async Task<IActionResult> RegisterSensor()
        {
            try
            {
                var sensors = await this.apiSensorsService.GetApiSensorsAsync();
                return Ok(sensors);
            }
                   
             catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
