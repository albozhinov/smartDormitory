using Microsoft.AspNetCore.Mvc;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Controllers
{
    public class SensorsController : Controller
    {
        private readonly IICBApiSensorsService apiSensorsService;

        public SensorsController(IICBApiSensorsService apiSensorsService)
        {
            this.apiSensorsService = apiSensorsService ?? throw new ArgumentNullException(nameof(apiSensorsService));
        }

        public async Task<IActionResult> RegisterSensor()
        {
            try
            {
                await this.apiSensorsService.UpdateSensorsAsync();
                return Ok();
            }

            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

        public IActionResult AllSensorTypes(List<SensorViewModel> model)
        {
            var sensors = this.apiSensorsService.ListAllSensors();

            foreach (var sensor in sensors)
            {
                model.Add(new SensorViewModel(sensor));
            }
            return View(model);
        }
    }
}
