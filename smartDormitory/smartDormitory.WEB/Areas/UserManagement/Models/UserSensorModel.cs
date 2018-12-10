using smartDormitory.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace smartDormitory.WEB.Areas.UserManagement.Models
{
    public class UserSensorModel
    {
        public UserSensorModel()
        {

        }
        public UserSensorModel(UserSensors userSensor)
        {
            this.Id = userSensor.Id;
            this.IcbSensorId = userSensor.Sensor.IcbSensorId;
            this.Tag = userSensor.Sensor.Tag;
            this.Name = userSensor.Name;
            this.Description = userSensor.Description;
            this.PollingInterval = userSensor.PollingInterval;
            this.TimeStamp = userSensor.Sensor.TimeStamp;
            this.Value = userSensor.Sensor.Value;
            this.MinValue = userSensor.MinValue;
            this.MaxValue = userSensor.MaxValue;
            this.ModifiedOn = userSensor.Sensor.ModifiedOn;
            this.IsPublic = userSensor.IsPublic;
            this.Alarm = userSensor.Alarm;
            this.Latitude = userSensor.Latitude;
            this.Longtitude = userSensor.Longitude;
        }

        public int Id { get; set; }

        public string IcbSensorId { get; set; }
        
        public string Tag { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(10, 40)]
        public int PollingInterval { get; set; }
        
        public DateTime TimeStamp { get; set; }

        [Range(0, 5000)]
        public double Value { get; set; }

        [Required]
        [Range(0, 5000)]
        public double MinValue { get; set; }

        [Required]
        [Range(0, 5000)]
        public double MaxValue { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        public bool IsPublic { get; set; }

        public bool Alarm { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }

        public double SensorTypeMinVal { get; set; }

        public double SensorTypeMaxVal { get; set; }
    }
}
