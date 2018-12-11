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
        private readonly IRoleManager<IdentityRole> _roleManager;
        private readonly int Page_Size = 3;

        [TempData]
        public string StatusMessage { get; set; }

        public UsersController(IUserManager<User> userManager, IUserService userService, IRoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this.userService = userService;
            this._roleManager = roleManager;
        }

        public async Task<IActionResult> AllUsers(AllUsersViewModel viewModel)
        {
            var isTextNull = viewModel.SearchText ?? "";

            var users = await this.userService.GetUsersAsync(isTextNull, 1, Page_Size);
            viewModel.Users = users.Select(u => new UserViewModel(u));

            var totalUsers = await this.userService.GetTotalUserAsync(isTextNull);

            viewModel.TotalPages = (int)Math.Ceiling(totalUsers / (double)Page_Size);

            return View(viewModel);
        }
                
        public async Task<IActionResult> UserGrid(AllUsersViewModel viewModel)
        {
            var isTextNull = viewModel.SearchText ?? "";
            var users = await this.userService.GetUsersAsync(isTextNull, viewModel.Page, Page_Size);

            viewModel.Users = users.Select(u => new UserViewModel(u));
            var totalUsers = await this.userService.GetTotalUserAsync(isTextNull);

            viewModel.TotalPages = (int)Math.Ceiling(totalUsers / (double)Page_Size);

            
            return PartialView("_UserGrid", viewModel);
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
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var lockOutTimeResult = await this._userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(10));

            if (!lockOutTimeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not add time to user's lockout!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            this.StatusMessage = "The user has been successfully locked for 10 years!";

            return View("_UserStatusMessage", this.StatusMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string Id)
        {
            var user =  _userManager.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (user is null)
            {
                this.StatusMessage = "Error: User not found!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var lockoutTimeResult = await this._userManager.SetLockoutEndDateAsync(user, DateTime.Now);

            if (!lockoutTimeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not add time to user's lockout!";
                return View("_UserStatusMessage", this.StatusMessage);
            }
            
            this.StatusMessage = "The user has been successfully unlocked!";
            return View("_UserStatusMessage", this.StatusMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserModalViewModel input)
        {
            var user = _userManager.Users.Where(u => u.Id == input.Id).FirstOrDefault();
            if (user is null)
            {
                this.StatusMessage = "Error: User not found!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            foreach (var validator in _userManager.PasswordValidators)
            {
                var result = await validator.ValidateAsync(_userManager.Instance, user, input.ConfirmPassword);
                if (!result.Succeeded)
                {
                    this.StatusMessage = $"Error: {string.Join(" ", result.Errors.Select(e => e.Description)).Replace(".", "!")}";
                    return View("_UserStatusMessage", this.StatusMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                this.StatusMessage = "Error: Passwords do not match!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not remove the old password!";
                return View("_UserStatusMessage", this.StatusMessage);
            }
            var addPasswordResult = await _userManager.AddPasswordAsync(user, input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not change the password!";
                return View("_UserStatusMessage", this.StatusMessage);
            }
            this.StatusMessage = "The user's password has been changed!";
            return View("_UserStatusMessage", this.StatusMessage);
        }

        [HttpPost]
        public async Task<JsonResult> GetRole(string Id)
        {
            var user = this._userManager.Users.Where(u => u.Id == Id).FirstOrDefault();
            if (user is null)
            {
                return new JsonResult("Error: User not found!");
            }

            var role = await this._userManager.GetRolesAsync(user);
            var myRole = role.FirstOrDefault();

            return new JsonResult(myRole);
        }

        [HttpGet]
        public JsonResult GetAllExistsRoles()
        {
            var roles = this._roleManager.Roles.ToList();

            return new JsonResult(roles);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string Id, string Role)
        {
            if (string.IsNullOrEmpty(Id))
            {
                this.StatusMessage = "Error: User's Id is incorrect!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            if (string.IsNullOrEmpty(Role))
            {
                this.StatusMessage = "Error: Role is incorrect!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var user = this._userManager.Users.Where(u => u.Id == Id).FirstOrDefault();

            if (user is null)
            {
                this.StatusMessage = "Error: User not found!";
                return View("_UserStatusMessage", this.StatusMessage);
            }
            
            var roleCheck = await this._roleManager.RoleExistsAsync(Role);
            if (!roleCheck)
            {
                this.StatusMessage = "Error: Role not found!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var userRole = await this._userManager.GetRolesAsync(user);
            var currentRole = userRole.FirstOrDefault();

            var removeResult = await this._userManager.RemoveFromRoleAsync(user, currentRole);
            if (!removeResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not remove the old role!";
                return View("_UserStatusMessage", this.StatusMessage);
            }

            var addRoleResult = await this._userManager.AddToRoleAsync(user, Role);
            if (!addRoleResult.Succeeded)
            {
                this.StatusMessage = "Error: Could not change the role!";
                return View("_UserStatusMessage", this.StatusMessage);
            }
            this.StatusMessage = "The user's role has been changed!";
            return View("_UserStatusMessage", this.StatusMessage);
        }
    }
}