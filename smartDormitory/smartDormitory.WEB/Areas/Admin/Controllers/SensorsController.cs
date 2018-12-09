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
        private readonly int Page_Size = 10;


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
    }
}