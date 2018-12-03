using smartDormitory.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace smartDormitory.WEB.Areas.UserManagement.Models
{
    public class UserSensorModel
    {
        public UserSensorModel(UserSensors userSensor)
        {
            this.IcbSensorId = userSensor.Sensor.IcbSensorId;
            this.Tag = userSensor.Sensor.Tag;
            this.Description = userSensor.Sensor.Description;
            this.PollingInterval = userSensor.PollingInterval;
            this.MeasureType = userSensor.Sensor.MeasureType;
            this.TimeStamp = userSensor.Sensor.TimeStamp;
            this.Value = userSensor.Sensor.Value;
            this.MinValue = userSensor.MinValue;
            this.MaxValue = userSensor.MaxValue;
            this.ModifiedOn = userSensor.Sensor.ModifiedOn;
            this.IsPublic = userSensor.IsPublic;
        }

        public string IcbSensorId { get; set; }
        
        public string Tag { get; set; }

        public string Description { get; set; }

        public int PollingInterval { get; set; }

        public MeasureType MeasureType { get; set; }

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
    }
}
