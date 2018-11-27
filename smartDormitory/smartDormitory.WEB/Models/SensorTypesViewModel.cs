using smartDormitory.Data;
using System.Collections.Generic;

namespace smartDormitory.WEB.Models
{
    public class SensorTypesViewModel
    {      
            public int TotalPages { get; set; }

            public int Page { get; set; } = 1;

            public int PreviousPage => this.Page ==
                1 ? 1 : this.Page - 1;

            public int NextPage => this.Page ==
                this.TotalPages ? this.TotalPages : this.Page + 1;

            public IEnumerable<Sensor> Sensors { get; set; }    

            public string SearchText { get; set; } = string.Empty;
    }
}
