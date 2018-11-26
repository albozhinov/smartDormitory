using Microsoft.EntityFrameworkCore;
using smartDormitory.Data;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services
{
    public class SensorService : ISensorService
    {
        private smartDormitoryDbContext context;

        public SensorService(smartDormitoryDbContext context)
        {
            this.context = context;
        }

        //public async Task<UserSensors> CreateSensorAsync(int sensorId, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        //{


        //    //public string UserId { get; set; }
        //    //public User User { get; set; }
        //    //public int SensorId { get; set; }
        //    //public Sensor Sensor { get; set; }
        //    //public double MinValue { get; set; }
        //    //public double MaxValue { get; set; }
        //    //public int PollingInterval { get; set; }
        //    //public double Latitude { get; set; }
        //    //public double Longitude { get; set; }
        //    //public bool IsPublic { get; set; }
        //    //public bool Alarm { get; set; }
        //}

        public async Task<IEnumerable<Sensor>> GetSensorsAsync()
        {
            return await this.context
                                .Sensors
                                .Include(s => s.MeasureType)
                                .ToListAsync();
        }



    }
}
