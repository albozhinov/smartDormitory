using smartDormitory.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace smartDormitory.Data
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IcbSensorId { get; set; }

        [Required]
        public string Tag { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int PollingInterval { get; set; }

        public int MeasureTypeId { get; set; }

        public MeasureType MeasureType { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public double MinValue { get; set; }

        [Required]
        public double MaxValue { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
