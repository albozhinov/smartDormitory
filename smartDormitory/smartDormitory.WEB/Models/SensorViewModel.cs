using smartDormitory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Models
{
    public class SensorViewModel
    {
        public SensorViewModel()
        {

        }

        public SensorViewModel(Sensor sensor)
        {
            this.Tag = sensor.Tag;
            this.Description = sensor.Description;
            this.MeasureType = sensor.MeasureType.Type;
        }

        public string Tag { get; set; }

        public string Description { get; set; }

        public string MeasureType { get; set; }
    }
}
