using smartDormitory.Data.Models;
using System;

namespace smartDormitory.Data
{
    public class Sensor
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        public string Tag { get; set; }

        public string Description { get; set; }

        public int PollingInterval { get; set; }

        public int MeasureTypeId { get; set; }

        public MeasureType MeasureType { get; set; }

        public DateTime TimeStamp { get; set; }

        public double Value { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public string Url { get; set; }
    }
}
