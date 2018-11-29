using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Data;
using smartDormitory.Data.Models;
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
        private readonly IUserSensorService userSensorService;
        private readonly UserManager<User> userManager;

        public SensorsController(IICBApiSensorsService apiSensorsService, IUserSensorService userSensorService, UserManager<User> userManager)
        {
            this.apiSensorsService = apiSensorsService ?? throw new ArgumentNullException(nameof(apiSensorsService));            
            this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> RegisterSensors()
        {
            try
            {
                await this.apiSensorsService.UpdateSensorsAsync();
                return Ok();
            }

            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting sensor information from ICBApi: {httpRequestException.Message}");
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

        [Authorize]
        [HttpGet]
        public IActionResult RegisterSensor(int sensorId, string description)
        {
            var userId = userManager.GetUserId(User);

            var userSensorModel = new UserSensorViewModel()
            {
                Id = sensorId,
                UserId = userId,
                Description = description
            };


            return View(userSensorModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RegisterSensor(UserSensorViewModel model)
        {
                if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            this.userSensorService.AddSensor(model.UserId, model.Id, model.MinValue, model.MaxValue,
                 model.PollingInterval, model.Latitude, model.Longtitude, model.IsPublic, model.Alarm);

            return RedirectToAction("Index", "Home");
        }

    }
}
