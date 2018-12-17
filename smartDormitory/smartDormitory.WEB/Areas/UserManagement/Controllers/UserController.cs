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
        private readonly IMailService mailService;

        [TempData]
        public string StatusMessage { get; set; }

        public UserController(IUserService userService, IUserSensorService userSensorService, UserManager<User> userManager, IMailService mailService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
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

        public async Task<IActionResult> UpdateSensors(UserAllSensorsModel allSensorsModel)
        {
            IEnumerable<UserSensors> userSensors = new List<UserSensors>();
            var userId = userManager.GetUserId(User);
            userSensors = await this.userSensorService.GetAllUserSensorsAsync(userId);
            allSensorsModel.UserSensors = userSensors.Select(s => new UserSensorModel(s));

            try
            {
                await mailService.SendEmail(userSensors, this.userManager.GetUserName(User), await this.userService.GetUserEmailAsync(userManager.GetUserId(User)));
            }
            catch (Exception)
            {//TODO: Log Errors
            }

            return PartialView("_MySensorGrid", allSensorsModel);
        }

        public IActionResult SensorGraphic(UserSensorModel viewModel)
        {
            return View(viewModel);
        }

        public async Task<JsonResult> UpdateSensor(string apiSensorId)
        {
            try
            {
                var sensor = await this.userSensorService.UpdateSensorValue(apiSensorId);
                var viewModel = new UserSensorModel(sensor);
                return new JsonResult(viewModel);
            }
            catch (Exception)
            {
                return new JsonResult("Error: Sensor data is incorrect!");
            }
        }
    }
}
