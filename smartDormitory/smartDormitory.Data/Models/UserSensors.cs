using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartDormitory.Data.Models
{
    public class UserSensors
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int SensorId { get; set; }

        public Sensor Sensor { get; set; }

        [Required]
        [Range(3,20)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0,5000)]
        public double MinValue { get; set; }

        [Required]
        [Range(0,5000)]
        public double MaxValue { get; set; }

        [Required]
        public int PollingInterval { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool Alarm { get; set; }
    }
}
