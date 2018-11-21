using smartDormitory.Data;
using System;

namespace smartDormitory.WEB.Models
{
    public class SensorsViewModel
    {

        public SensorsViewModel(Sensor sensor)
        {
            this.SensorId = sensor.Guid;
            this.Tag = sensor.Tag;
            this.Description = sensor.Description;
            this.MinPollingIntervalInSeconds = sensor.PollingInterval;
            this.MeasureType = sensor.MeasureType.Type;
            this.TimeStamp = sensor.TimeStamp;
            this.Value = sensor.Value;

        }

        public string SensorId { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int MinPollingIntervalInSeconds { get; set; }
        public string MeasureType { get; set; }
        public DateTime TimeStamp { get; set; }
        public double    Value { get; set; }
    }

}
