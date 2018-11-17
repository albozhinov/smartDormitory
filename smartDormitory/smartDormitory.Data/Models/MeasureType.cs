using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Data.Models
{
    public class MeasureType
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}
