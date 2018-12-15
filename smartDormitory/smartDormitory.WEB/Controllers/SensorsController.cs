using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Controllers
{
    public class SensorsController : Controller
    {
        private readonly IICBApiSensorsService apiSensorsService;
        private readonly IUserSensorService userSensorService;
        private readonly UserManager<User> userManager;
        private const int Page_Size = 5;

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
                model.Sensors = this.apiSensorsService.ListAllSensors(model.Page, Page_Size);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.Total() / (double)Page_Size);
            }
            else
            {
                model.Sensors = this.apiSensorsService.ListByContainingText(model.SearchText, model.Page, Page_Size);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.TotalContainingText(model.SearchText) / (double)Page_Size);
            }

            return View(model);
        }

        [Authorize]
        public IActionResult AllSensorTypesGrid(SensorTypesViewModel model)
        {
            if (model.SearchText == null)
            {
                model.Sensors = this.apiSensorsService.ListAllSensors(model.Page, Page_Size);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.Total() / (double)Page_Size);
            }
            else
            {
                model.Sensors = this.apiSensorsService.ListByContainingText(model.SearchText, model.Page, Page_Size);
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.TotalContainingText(model.SearchText) / (double)Page_Size);
            }

            return PartialView("_AllSensorTypesGrid", model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RegisterSensor(int sensorId, string tag, string description)
        {
            var userId = userManager.GetUserId(User);

            var validationsMinMax = this.GetMinMaxValidations(description);

            var userSensorModel = new UserSensorViewModel()
            {
                Id = sensorId,
                UserId = userId,
                Tag = tag,
                ValidationsMinMax = validationsMinMax
            };

            return View(userSensorModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSensor(UserSensorViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            await this.userSensorService.AddSensorAsync(model.UserId, model.Id, model.Name, model.Description, model.MinValue, model.MaxValue,
                 model.PollingInterval, model.Latitude, model.Longtitude, model.IsPublic, model.Alarm, model.ImageUrl);

            return RedirectToAction("Index", "Home");
        }

        private List<double> GetMinMaxValidations(string description)
        {
            var validations = new List<double>();

            string[] numbers = Regex.Split(description, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    validations.Add(double.Parse(value));
                }
            }
            return validations;
        }
    }
}
