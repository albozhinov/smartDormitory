using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.UserManagement.Controllers
{
    [Area("UserManagement")]
    [Route("User/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [Route("[action]/{id}")]
        public IActionResult MySensors(string id)
        {
            var sensors = userService.GetAllUserSensors(id);
            return View();
        }
    }
}
