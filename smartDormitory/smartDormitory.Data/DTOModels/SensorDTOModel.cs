using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Data.DTOModels
{
    public class SensorDTOModel
    {
        public string SensorId { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public int MinPollingIntervalInSeconds { get; set; }
        public string MeasureType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
