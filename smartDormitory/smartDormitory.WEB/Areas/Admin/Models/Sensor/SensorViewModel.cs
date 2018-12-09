using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.Admin.Models.Sensor
{
    public class SensorViewModel
    {
        public SensorViewModel(UserSensors userSensors)
        {
            this.Id = userSensors.Id;
            this.Name = userSensors.Name;
            this.UserName = userSensors.User.UserName;
            this.Description = userSensors.Description;
            this.Tag = userSensors.Sensor.Tag;
            this.Value = userSensors.Sensor.Value;
            this.ModifiedOn = userSensors.Sensor.ModifiedOn;
            this.Alarm = userSensors.Alarm;
            this.Latitude = userSensors.Latitude;
            this.Longtitude = userSensors.Longitude;
            this.URL = userSensors.Sensor.Url;
        }
        
        public int Id { get; set; }

        public string UserId { get; set; }

        public int SensorId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(0, 5000)]
        public double MinValue { get; set; }

        [Range(0, 5000)]
        public double MaxValue { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(10, 40)]
        public int PollingInterval { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longtitude { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool Alarm { get; set; }

        public List<double> ValidationsMinMax { get; set; }

        public string Tag { get; set; }

        public string UserName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public double Value { get; set; }

        public string URL { get; set; }
    }
}
