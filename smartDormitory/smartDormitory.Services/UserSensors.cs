using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Data.Models
{
    public class UserSensors
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int SensorId { get; set; }

        public Sensor Sensor { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public int PollingInterval { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsPublic { get; set; }

        public bool Alarm { get; set; }
    }
}
