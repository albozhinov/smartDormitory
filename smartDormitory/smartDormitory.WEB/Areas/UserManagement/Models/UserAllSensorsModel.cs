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

        public IEnumerable<UserSensorModel> UserSensors { get; set; }
 
    }
}
