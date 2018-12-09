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

        public void AddSensor(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("User Id cannot be null!");
            }

            if (sensorId < 0)
            {
                throw new ArgumentException("Sensor Id cannot be less than 0!");
            }

            if (name == null)
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (name.Length < 3 || name.Length > 20)
            {
                throw new ArgumentException("Name must be between 3 and 20 symbols!");
            }

            if (description == null)
            {
                throw new ArgumentNullException("Description cannot be null!");
            }

            var sensor = this.context.Sensors
                .Where(s => s.Id == sensorId)
                .ToList();

            if (sensor == null || sensor.Count < 1)
            {
                throw new ArgumentNullException($"Sensor with Id {sensorId} does not exist!");
            }

            if (minValue < sensor[0].MinValue || minValue > sensor[0].MaxValue)
            {
                throw new ArgumentException($"Minimal value must be between {sensor[0].MinValue} and {sensor[0].MaxValue}symbols!");
            }

            if (maxValue < sensor[0].MinValue || maxValue > sensor[0].MaxValue)
            {
                throw new ArgumentException($"Maximal value must be between {sensor[0].MinValue} and {sensor[0].MaxValue} symbols!");
            }

            if (pollingInterval < 10 || pollingInterval > 40)
            {
                throw new ArgumentException("Polling interval must be between 10 and 40 symbols!");
            }

            //if (latitude <= 0)
            //{
            //    throw new ArgumentException("Latitude cannot be less than 0!");
            //}

            //if (longitude <= 0)
            //{
            //    throw new ArgumentException("Longitude cannot be less than 0!");
            //}

            if (sensor == null)
            {
                throw new ArgumentNullException("Sensor does not exist!");
            }

            var newUserSensor = new UserSensors
            {
                SensorId = sensorId,
                UserId = userId,
                Name = name,
                Description = description,
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

        public async Task<IEnumerable<UserSensors>> GetAllPublicUsersSensorsAsync()
        {
            return await this.context
                                .UserSensors
                                .Where(s => s.IsPublic)
                                .Include(u => u.User)
                                .Include(s => s.Sensor)
                                .ToListAsync();
        }

        public async Task<IEnumerable<double>> GetSensorsTypeMinMaxValues(string tag)
        {
            var minMax = await this.context.Sensors
                .Where(s => s.Tag.Contains(tag.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                .Select(s => new
                {
                    s.MinValue,
                    s.MaxValue
                })
               .ToListAsync();

            return new List<double>{minMax[0].MinValue, minMax[0].MaxValue };
        public async Task<IEnumerable<UserSensors>> GetAllUserSensorsByContainingTagAsync(string id, string searchText, int page = 1, int pageSize = 10)
        {
            return await this.context.UserSensors
                .Where(us => us.UserId == id)
                .Where(us => us.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .Include(s => s.Sensor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id, int page = 1, int pageSize = 10)
        {
            return await this.context.UserSensors
                .Where(us => us.UserId == id)
                .Include(s => s.Sensor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public int TotalContainingText(string searchText)
        {
            return this.context.UserSensors
                .Where(s => s.Sensor.Tag.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .Count();
        }

        public int Total()
        {
            return this.context.UserSensors.Count();
        }

        public async Task<IEnumerable<UserSensors>> GetAllPrivateUserSensorsAsync(string id)
        {
            return await this.context.UserSensors
                                     .Where(s => s.UserId == id && !s.IsPublic)
                                     .Include(u => u.User)
                                     .Include(s => s.Sensor)
                                     .ToListAsync();
        }

        public async Task<IEnumerable<UserSensors>> GetAllUsersSensorsAsync(string searchByName, string searchByTag, int page = 1, int pageSize = 10)
        {
            return await this.context.UserSensors
                .Where(us => us.User.UserName.Contains(searchByName, StringComparison.InvariantCultureIgnoreCase))
                .Where(us => us.Sensor.Tag.Contains(searchByTag, StringComparison.InvariantCultureIgnoreCase))
                .Include(s => s.Sensor)
                .Include(u => u.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public int TotalByName(string textName)
        {
            return this.context.UserSensors.Where(s => s.User.UserName
                                               .Contains(textName, StringComparison.InvariantCultureIgnoreCase))
                                           .Include(s => s.User)
                                           .Count();
        }
    }
}
