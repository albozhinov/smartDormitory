using smartDormitory.Data.Models;

namespace smartDormitory.WEB.Areas.Admin.Models
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
