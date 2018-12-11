using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Areas.UserManagement.Models;
using smartDormitory.WEB.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserSensorService userSensorService;
        private readonly UserManager<User> userManager;
        private readonly int Page_Size = 10;

        [TempData]
        public string StatusMessage { get; set; }

        public UserController(IUserService userService, IUserSensorService userSensorService, UserManager<User> userManager)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        public async Task<IActionResult> MySensors(UserAllSensorsModel userSensorsModel)
        {
            IEnumerable<UserSensors> userSensors = new List<UserSensors>();
            userSensors = await this.userSensorService.GetAllUserSensorsAsync(userManager.GetUserId(User));
            userSensorsModel.UserSensors = userSensors.Select(s => new UserSensorModel(s));

            return View(userSensorsModel);
        }

        [HttpGet]
        public async Task<IActionResult> ModifySensor(UserSensorModel mySensorModel)
        {
            var sensorMinMaxValuesList = new List<double>(await this.userSensorService.GetSensorsTypeMinMaxValues(mySensorModel.Tag));

            mySensorModel.SensorTypeMinVal = sensorMinMaxValuesList[0];

            mySensorModel.SensorTypeMaxVal = sensorMinMaxValuesList[1];

            return View(mySensorModel);
        }

        [HttpPost]
        public async Task<IActionResult> ModifySensorPost(UserSensorModel model)
        {
            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error: The data is incorrect!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            try
            {
                await this.userSensorService.EditSensor(model.Id, model.IcbSensorId, model.Name, model.Description, model.MinValue, model.MaxValue, model.PollingInterval, model.Latitude, model.Longtitude, model.IsPublic, model.Alarm);
            }
            catch (Exception)
            {
                this.StatusMessage = "Error: The data is incorrect!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            this.StatusMessage = "The sensor has been successfully edited!";
            return View("_EditStatusMessage", this.StatusMessage);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSensor(string name)
        {
            try
            {
                await this.userSensorService.DeleteSensor(name);
            }
            catch (Exception)
            {
                this.StatusMessage = "Error: Sensor could not be deleted!";
                return View("_EditStatusMessage", this.StatusMessage);
            }

            this.StatusMessage = "The sensor has been successfully deleted!";
            return View("_EditStatusMessage", this.StatusMessage);
        }
    }
}
