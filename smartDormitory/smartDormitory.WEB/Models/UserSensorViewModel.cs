using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartDormitory.WEB.Models
{
    public class UserSensorViewModel
    {
        public UserSensorViewModel()
        {

        }


        public int Id { get; set; }

        public string UserId { get; set; }
        
        public int SensorId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(0,5000)]
        public double MinValue { get; set; }

        [Range(0, 5000)]
        public double MaxValue { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(10,40)]
        public int PollingInterval { get; set; }

        public double Latitude { get; set; }

        public double Longtitude { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public bool Alarm { get; set; }

        public List<double> ValidationsMinMax { get; set; }

        public string Tag { get; set; }

        public string UserName { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public double Value { get; set; }

        public string URL { get; set; }

        public string ImageUrl { get; set; }
    }
}
