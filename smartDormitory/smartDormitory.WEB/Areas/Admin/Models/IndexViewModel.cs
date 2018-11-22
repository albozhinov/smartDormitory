using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace smartDormitory.WEB.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<User> users, int pageNumber, int pageSize)
        {
            this.Users = users.Select(u => new UserViewModel(u)).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<UserViewModel> Users { get; set; }
    }
}
