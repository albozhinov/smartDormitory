using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Areas.Admin.Models.Sensor;

namespace smartDormitory.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SensorsController : Controller
    {
        private readonly IUserSensorService userSensorService;
        private readonly IICBApiSensorsService apiSensorsService;
        private readonly int Page_Size = 4;

        [TempData]
        public string StatusMessage { get; set; }


        public SensorsController(IUserSensorService userSensorService)
        {
            this.userSensorService = userSensorService;
        }

        public async Task<IActionResult> AllSensors(AllSensorsViewModel viewModel)
        {
            var isTagTextNull = viewModel.SearchByTag ?? "";
            var isNameTextNull = viewModel.SearchByName ?? "";

            var sensors = await this.userSensorService.GetAllUsersSensorsAsync(isNameTextNull, isTagTextNull, viewModel.Page, Page_Size);

            viewModel.Sensors = sensors.Select(s => new SensorViewModel(s));
            if (string.IsNullOrEmpty(isTagTextNull) && string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.Total();
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isTagTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalContainingText(isTagTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalByName(isNameTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> SensorGrid(AllSensorsViewModel viewModel)
        {
            var isTagTextNull = viewModel.SearchByTag ?? "";
            var isNameTextNull = viewModel.SearchByName ?? "";

            var sensors = await this.userSensorService.GetAllUsersSensorsAsync(isNameTextNull, isTagTextNull, viewModel.Page, Page_Size);

            viewModel.Sensors = sensors.Select(s => new SensorViewModel(s));
            if (string.IsNullOrEmpty(isTagTextNull) && string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.Total();
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isTagTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalContainingText(isTagTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalByName(isNameTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }

            return PartialView("_SensorGrid", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditSensor(SensorViewModel viewModel)
        {
            var sensorMinMaxValuesList = new List<double>(await this.userSensorService.GetSensorsTypeMinMaxValues(viewModel.Tag));

            viewModel.SensorTypeMinVal = sensorMinMaxValuesList[0];

            viewModel.SensorTypeMaxVal = sensorMinMaxValuesList[1];

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSensorPost(SensorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error: The data is incorrect!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            try
            {
                await this.userSensorService.EditSensor(viewModel.Id, viewModel.IcbSensorId, viewModel.Name, viewModel.Description, viewModel.MinValue, viewModel.MaxValue, viewModel.PollingInterval, viewModel.Latitude, viewModel.Longtitude, viewModel.IsPublic, viewModel.Alarm);
            }
            catch (Exception)
            {
                this.StatusMessage = "Error: The data is incorrect!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            this.StatusMessage = "The sensor has been successfully edited!";
            return View("_EditStatusMessage", this.StatusMessage);
        }

        public IActionResult SensorGraphic(SensorViewModel viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSensor(string apiSensorId)
        {
            try
            {
                var sensor = await this.userSensorService.UpdateSensorValue(apiSensorId);
                var viewModel = new SensorViewModel() {
                    Value = sensor.Value,
                    ModifiedOn = sensor.ModifiedOn,
                };
                return new JsonResult(viewModel);
            }
            catch (Exception)
            {
               return new JsonResult("Error: Sensor data is incorrect!");
            }
        }

        public async Task<IActionResult> UpdateAllSensors(AllSensorsViewModel viewModel)
        {
            var isTagTextNull = viewModel.SearchByTag ?? "";
            var isNameTextNull = viewModel.SearchByName ?? "";

            var sensors = await this.userSensorService.GetAllUsersSensorsAsync(isNameTextNull, isTagTextNull, viewModel.Page, Page_Size);

            viewModel.Sensors = sensors.Select(s => new SensorViewModel() {
                IcbSensorId = s.Sensor.IcbSensorId,
                ModifiedOn = s.Sensor.ModifiedOn,
                Value = s.Sensor.Value,
                Tag = s.Sensor.Tag,
            });
            if (string.IsNullOrEmpty(isTagTextNull) && string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.Total();
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isTagTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalContainingText(isTagTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }
            else if (!string.IsNullOrEmpty(isNameTextNull))
            {
                var totalUserSensors = this.userSensorService.TotalByName(isNameTextNull);
                viewModel.TotalPages = (int)Math.Ceiling(totalUserSensors / (double)Page_Size);
            }

            return Json(viewModel);
        }

        public async Task<IActionResult> AllPrivateAndPublicUsersSensors()
        {
            var sensors = await this.userSensorService.GetAllPublicAndPrivateUsersSensorsAsync();
            var sensorsViewModel = sensors.Select(s => new SensorViewModel()
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            try
            {
                await this.userSensorService.DeleteSensor(id);
            }
            catch (Exception)
            {
                this.StatusMessage = "Error: Sensor could not be deleted!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            this.StatusMessage = "The sensor has been successfully deleted!";
            return View("_EditStatusMessage", this.StatusMessage);
        }

        public IActionResult AllSensorTypes(AllSensorsViewModel model)
        {
            if (model.SearchByTag == null)
            {
                var dataSensors  = this.apiSensorsService.ListAllSensors(model.Page, Page_Size);
                model.Sensors = dataSensors.Select(s => new SensorViewModel(s)).ToList();
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.Total() / (double)Page_Size);
            }
            else
            {
                var dataSensors = this.apiSensorsService.ListByContainingText(model.SearchByTag, model.Page, Page_Size);
                model.Sensors = dataSensors.Select(s => new SensorViewModel(s)).ToList();
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.TotalContainingText(model.SearchByTag) / (double)4);
            }

            return View(model);
        }
        
        public IActionResult AllSensorTypesGrid(AllSensorsViewModel model)
        {
            if (model.SearchByTag == null)
            {
                var dataSensors = this.apiSensorsService.ListAllSensors(model.Page, Page_Size);
                model.Sensors = dataSensors.Select(s => new SensorViewModel(s)).ToList();
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.Total() / (double)Page_Size);
            }
            else
            {
                var dataSensors = this.apiSensorsService.ListByContainingText(model.SearchByTag, model.Page, Page_Size);
                model.Sensors = dataSensors.Select(s => new SensorViewModel(s)).ToList();
                model.TotalPages = (int)Math.Ceiling(this.apiSensorsService.TotalContainingText(model.SearchByTag) / (double)4);
            }

            return PartialView("_AllSensorTypesGrid", model);
        }
        
        public IActionResult RegisterSensor(string userId, int sensorId, string tag, string description)
        {
            var validationsMinMax = this.GetMinMaxValidations(description);

            var userSensorModel = new SensorViewModel()
            {
                Id = sensorId,
                UserId = userId,
                Tag = tag,
                ValidationsMinMax = validationsMinMax
            };

            return View(userSensorModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSensor(SensorViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            await this.userSensorService.AddSensor(model.UserId, model.Id, model.Name, model.Description, model.MinValue, model.MaxValue,
                 model.PollingInterval, model.Latitude, model.Longtitude, model.IsPublic, model.Alarm, model.ImageURL);

            return RedirectToAction("Index", "Home", new { area = "" });
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