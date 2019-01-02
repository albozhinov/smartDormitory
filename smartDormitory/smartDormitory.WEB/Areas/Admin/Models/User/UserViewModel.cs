using smartDormitory.Data.Models;
using System;

namespace smartDormitory.WEB.Areas.Admin.Models
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.LockoutEnd = user.LockoutEnd;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
