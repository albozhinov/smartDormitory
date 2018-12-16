using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartDormitory.Data.Models
{
    public class MeasureType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}
