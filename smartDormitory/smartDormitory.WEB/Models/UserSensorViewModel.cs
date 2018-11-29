using smartDormitory.Data;
using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Models
{
    public class UserSensorViewModel
    {
        public UserSensorViewModel()
        {

        }


        public int Id { get; set; }

        public string UserId { get; set; }
        
        public int SensorId { get; set; }

        public string Tag { get; set; }

        [Range(0,5000)]
        public double MinValue { get; set; }

        [Range(0, 5000)]
        public double MaxValue { get; set; }

        public string Description { get; set; }

        public int PollingInterval { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }

        public bool IsPublic { get; set; }

        public bool Alarm { get; set; }

        public List<double> ValidationsMinMax { get; set; }
    }
}
