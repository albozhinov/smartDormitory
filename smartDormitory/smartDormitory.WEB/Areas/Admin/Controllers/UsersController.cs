using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Data.Models;
using smartDormitory.WEB.Areas.Admin.Models;
using smartDormitory.WEB.Providers;
using X.PagedList;

namespace smartDormitory.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserManager<User> _userManager;
        private readonly int Page_Size = 1;

        public UsersController(IUserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            // TODO: Create UserService to Include all information to user (role, sensorsCount, sensortsType and ect.)
            var indexViewModel = new IndexViewModel(_userManager.Users, 1, Page_Size);
            return View(indexViewModel);
        }

        public IActionResult UserGrid(int? page)
        {
            var pagedUsers = this._userManager
                                 .Users
                                 .Select(u => new UserViewModel(u))
                                 .ToPagedList(page ?? 1, Page_Size);

            return PartialView("_UserGrid", pagedUsers);
        }
    }
}