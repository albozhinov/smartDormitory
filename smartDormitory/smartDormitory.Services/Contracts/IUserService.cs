using Microsoft.AspNetCore.Identity;
using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services.Contracts
{
    public interface IUserService
    {
        Task<ICollection<User>> GetUsersAsync(string searchText,int page = 1, int pageSize = 5);

        Task<int> GetTotalUserAsync(string searchText);

        IEnumerable<UserSensors> GetAllUserSensors(string id);
    }
}
