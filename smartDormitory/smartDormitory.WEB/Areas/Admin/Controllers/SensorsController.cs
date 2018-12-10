using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly int Page_Size = 8;

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

            return PartialView("_SensorGrid" ,viewModel);
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
    }
}