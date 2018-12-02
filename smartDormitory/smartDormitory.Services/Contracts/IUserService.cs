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

        Task<IEnumerable<UserSensors>> GetAllUserSensorsByContainingTagAsync(string id, string searchText, int page = 1, int pageSize = 10);

        Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id, int page = 1, int pageSize = 10);

        int Total();

        int TotalContainingText(string searchText);
    }
}
