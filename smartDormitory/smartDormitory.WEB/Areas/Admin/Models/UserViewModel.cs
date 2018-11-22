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
        public IEnumerable<User> Users { get; set; }

        // Pagination Properties
        public int TotalPages { get; set; }
        public int Page { get; set; } = 1;
        public int PreviousPage => this.Page ==
            1 ? 1 : this.Page - 1;
        public int NextPage => this.Page ==
            this.TotalPages ? this.TotalPages : this.Page + 1;
        public string SearchText { get; set; } = string.Empty;
    }
}
