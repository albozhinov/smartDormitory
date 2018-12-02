using smartDormitory.Data;
using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.UserManagement.Models
{
    public class UserAllSensorsModel
    {
        public UserAllSensorsModel()
        {

        }
        public UserAllSensorsModel(IEnumerable<UserSensors> sensors)
        {
            this.UserSensors = sensors.Select(s => new UserSensorModel(s));
        }

        public int TotalPages { get; set; }

        public int Page { get; set; } = 1;

        public int PreviousPage => this.Page ==
            1 ? 1 : this.Page - 1;

        public int NextPage => this.Page ==
            this.TotalPages ? this.TotalPages : this.Page + 1;

        public IEnumerable<UserSensorModel> UserSensors { get; set; }

        public string SearchText { get; set; } = string.Empty;
    }
}
