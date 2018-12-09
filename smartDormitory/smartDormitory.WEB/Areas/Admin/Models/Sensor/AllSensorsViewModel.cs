using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Areas.Admin.Models.Sensor
{
    public class AllSensorsViewModel
    {
        public IEnumerable<SensorViewModel> Sensors { get; set; }

        // Pagination Properties        
        public int TotalPages { get; set; }
        public int Page { get; set; } = 1;
        public int PreviousPage => this.Page ==
            1 ? 1 : this.Page - 1;
        public int NextPage => this.Page ==
            this.TotalPages ? this.TotalPages : this.Page + 1;
        public string SearchByTag { get; set; } = string.Empty;
        public string SearchByName { get; set; } = string.Empty;
        public string StatusMessage { get; set; }
    }
}
