using System;

namespace smartDormitory.WEB.Models
{
    public class SensorsViewModel
    {

        public string SensorId { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int MinPollingIntervalInSeconds { get; set; }
        public string MeasureType { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Value { get; set; }
    }

}
