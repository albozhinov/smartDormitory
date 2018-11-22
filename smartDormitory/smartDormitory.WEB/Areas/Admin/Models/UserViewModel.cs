using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.Admin.Models
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.ID = user.Id;
        }

        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string ID { get; set; }
    }
}
