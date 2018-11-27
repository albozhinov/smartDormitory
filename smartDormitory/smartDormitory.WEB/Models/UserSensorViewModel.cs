using smartDormitory.Data;
using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Models
{
    public class UserSensorViewModel
    {
        public UserSensorViewModel()
        {

        }

        public UserSensorViewModel(Sensor sensor, User user)
        {
            sensor.Id = Id;
            UserId = UserId;
        }

        public int Id { get; set; }

        public string UserId { get; set; }
        
        public int SensorId { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public int PollingInterval { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }

        public bool IsPublic { get; set; }

        public bool Alarm { get; set; }
    }
}
