using Microsoft.EntityFrameworkCore;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services
{
    public class UserSensorService : IUserSensorService
    {
        private readonly smartDormitoryDbContext context;

        public UserSensorService(smartDormitoryDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<UserSensors> GetAllUserSensors(int sensorId, string userId)
        {
            if(sensorId < 0)
            {
                throw new ArgumentException("Sensor Id cannot be less than 0!");
            }

            if(userId == null)
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            return this.context.UserSensors
                  .Where(s => s.SensorId == sensorId)
                  .Include(s => s.Sensor)
                  .Where(s => s.UserId == userId)
                  .ToList();
        }

        public void AddSensor(string userId, int sensorId, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            if(userId == null)
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            if(sensorId < 0)
            {
                throw new ArgumentException("Sensor Id cannot be less than 0!");
            }

            var sensor = this.context.Sensors
                .Where(s => s.Id == sensorId)
                .ToList();

            if(sensor == null || sensor.Count < 1)
            {
                throw new ArgumentNullException($"Sensor with Id {sensorId} does not exist!");
            }

            if(minValue < sensor[0].MinValue || minValue > sensor[0].MaxValue)
            {
                throw new ArgumentException($"Minimal value must be between {sensor[0].MinValue} and {sensor[0].MaxValue}!");
            }

            if (maxValue < sensor[0].MinValue || maxValue > sensor[0].MaxValue)
            {
                throw new ArgumentException($"Maximal value must be between {sensor[0].MinValue} and {sensor[0].MaxValue}!");
            }

            if(pollingInterval < 0)
            {
                throw new ArgumentException("Polling interval cannot be less than 0!");
            }

            if (latitude <= 0)
            {
                throw new ArgumentException("Latitude cannot be less than 0!");
            }

            if (longitude <= 0)
            {
                throw new ArgumentException("Longitude cannot be less than 0!");
            }

            if (sensor == null)
            {
                throw new ArgumentNullException("Sensor does not exist!");
            }

            var newUserSensor = new UserSensors
            {
                SensorId = sensorId,
                UserId = userId,
                MinValue = minValue,
                MaxValue = maxValue,
                PollingInterval = pollingInterval,
                Latitude = latitude,
                Longitude = longitude,
                IsPublic = isPublic,
                Alarm = alarm,
            };

            this.context.UserSensors.Add(newUserSensor);
            this.context.SaveChanges();
        }

        public async Task<IEnumerable<UserSensors>> GetAllPublicUsersSensors()
        {
            return await this.context
                                .UserSensors
                                .Where(s => s.IsPublic)
                                .ToListAsync();
        }
    }
}
