using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult AllSensorTypes(SensorTypesViewModel model)
        {
            if (model.SearchText == null)
            {
                model.Sensors = this.apiSensorsService.ListAllSensors(model.Page, 10);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.Total() / (double)10);
            }
            else
            {
                model.Sensors = this.apiSensorsService.ListByContainingText(model.SearchText, model.Page, 10);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.TotalContainingText(model.SearchText) / (double)10);
            }

            return View(model);
        }
    }
}
