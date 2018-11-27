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

        [TempData]
        public string StatusMessage { get; set; }

        public UsersController(IUserManager<User> userManager, IUserService userService)
        {
            this._userManager = userManager;
            this.userService = userService;
        }

        public async Task<IActionResult> AllUsers(AllUsersViewModel viewModel)
        {
            // TODO: Create UserService to Include all information to user (role, sensorsCount, sensortsType and ect.)
            var isTextNull = viewModel.SearchText ?? "";
            var users = await this.userService.GetUsersAsync(isTextNull, viewModel.Page, Page_Size);

            viewModel.Users = users.Select(u => new UserViewModel(u));
            var totalUsers = await this.userService.GetTotalUserAsync(isTextNull);

            viewModel.TotalPages = (int)Math.Ceiling(totalUsers / (double)Page_Size);

            return View(viewModel);
        }       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string Id)
        {
            var user = this._userManager.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (user is null)
            {
                this.StatusMessage = "Error: User not found!";
                return RedirectToAction(nameof(AllUsers));
            }

            var enableLockOutResult = await this._userManager.SetLockoutEnabledAsync(user, true);

            if (!enableLockOutResult.Succeeded)
            {
                this.StatusMessage = "Error: Could enable the lockout on the user!";
                return RedirectToAction(nameof(AllUsers));
            }

            var lockOutTimeResult = await this._userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(10));

            if (!lockOutTimeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not add time to user's lockout!";
                return RedirectToAction(nameof(AllUsers));
            }

            this.StatusMessage = "The user has been successfully locked for 10 years!";

            return RedirectToAction(nameof(AllUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string Id)
        {
            var user =  _userManager.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (user is null)
            {
                this.StatusMessage = "Error: User not found!";
                return RedirectToAction(nameof(AllUsers));
            }

            var lockoutTimeResult = await this._userManager.SetLockoutEndDateAsync(user, DateTime.Now);

            if (!lockoutTimeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not add time to user's lockout!";
                return this.RedirectToAction(nameof(AllUsers));
            }

            this.StatusMessage = "The user has been successfully unlocked!";
            return RedirectToAction(nameof(AllUsers));
        }
    }
}