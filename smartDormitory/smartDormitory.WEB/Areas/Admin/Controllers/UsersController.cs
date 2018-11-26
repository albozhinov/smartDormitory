using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Areas.Admin.Models;
using smartDormitory.WEB.Providers;

namespace smartDormitory.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserManager<User> _userManager;
        private readonly int Page_Size = 3;

        public UsersController(IUserManager<User> userManager, IUserService userService)
        {
            this._userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> AllUsers(UserViewModel viewModel)
        {
            // TODO: Create UserService to Include all information to user (role, sensorsCount, sensortsType and ect.)
            var isTextNull = viewModel.SearchText ?? "";
            viewModel.Users = await this.userService.GetUsersAsync(isTextNull, viewModel.Page, Page_Size);
            var totalUsers = await this.userService.GetTotalUserAsync(isTextNull);

            viewModel.TotalPages = (int)Math.Ceiling(totalUsers / (double)Page_Size);

            return View(viewModel);
        }       
    }
}