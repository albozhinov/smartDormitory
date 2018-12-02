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
        private readonly UserManager<User> userManager;
        private readonly int Page_Size = 10;

        public UserController(IUserService userService, UserManager<User> userManager)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        public async Task<IActionResult> MySensors(UserAllSensorsModel userSensorsModel)
        {
            IEnumerable<UserSensors> userSensors = new List<UserSensors>();
            if (userSensorsModel.SearchText == null || userSensorsModel.SearchText == "")
            {
                userSensors = await this.userService.GetAllUserSensorsAsync(userManager.GetUserId(User), userSensorsModel.Page, Page_Size);
                userSensorsModel.TotalPages = (int)Math.Ceiling(this.userService.Total() / (double)Page_Size);
            }

            else
            {
               
                userSensors = await this.userService.GetAllUserSensorsByContainingTagAsync(userManager.GetUserId(User), userSensorsModel.SearchText, userSensorsModel.Page, 10);
                userSensorsModel.TotalPages = (int)Math.Ceiling(this.userService.TotalContainingText(userSensorsModel.SearchText) / (double)10);
            }

            userSensorsModel.UserSensors = userSensors.Select(s => new UserSensorModel(s));

            return View(userSensorsModel);
        }
    }
}
