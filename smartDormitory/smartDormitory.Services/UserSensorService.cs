using Microsoft.EntityFrameworkCore;
using smartDormitory.Data;
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

        public async Task AddSensor(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm, string imageUrl)
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

            var sensor = await this.context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);                

            if (sensor is null)
            {
                throw new ArgumentNullException($"Sensor with Id {sensorId} does not exist!");
            }

            if (minValue < sensor.MinValue || minValue > sensor.MaxValue)
            {
                throw new ArgumentException($"Minimal value must be between {sensor.MinValue} and {sensor.MaxValue}!");
            }

            if (maxValue < sensor.MinValue || maxValue > sensor.MaxValue)
            {
                throw new ArgumentException($"Maximal value must be between {sensor.MinValue} and {sensor.MaxValue}!");
            }

            if (pollingInterval < 10 || pollingInterval > 40)
            {
                throw new ArgumentException("Polling interval must be between 10 and 40 seconds!");
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentNullException("Image URL cannot be null or empty!");
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
                ImageUrl = imageUrl,
                IsDeleted = false
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

            return new List<double> { minMax[0].MinValue, minMax[0].MaxValue };
        }

        public async Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id)
        {
            return await this.context.UserSensors
                .Where(us => us.UserId == id && !us.IsDeleted)
                .Include(s => s.Sensor)
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
                    .ThenInclude(s => s.MeasureType)
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

        public async Task EditSensor(int userSensorId, string icbSensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm)
        {   

            if (userSensorId <= 0 )
            {
                throw new ArgumentException("Sensor Id cannot be less or equal 0!");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name cannot be null!");
            }

            if (name.Length < 3 || name.Length > 20)
            {
                throw new ArgumentException("Name must be between 3 and 20 symbols!");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException("Description cannot be null!");
            }

            var sensor = await this.context.Sensors.FirstOrDefaultAsync(s => s.IcbSensorId == icbSensorId);               

            if (sensor is null)
            {
                throw new ArgumentNullException($"ICB Sensor with Id {icbSensorId} does not exist!");
            }

            if (minValue < sensor.MinValue || minValue > sensor.MaxValue)
            {
                throw new ArgumentException($"Minimal value must be between {sensor.MinValue} and {sensor.MaxValue}!");
            }

            if (maxValue < sensor.MinValue || maxValue > sensor.MaxValue)
            {
                throw new ArgumentException($"Maximal value must be between {sensor.MinValue} and {sensor.MaxValue}!");
            }

            if (pollingInterval < 10 || pollingInterval > 40)
            {
                throw new ArgumentException("Polling interval must be between 10 and 40 seconds!");
            }

            var userSensorToUpdate = await this.context.UserSensors.FirstOrDefaultAsync(s => s.Id == userSensorId);

            if (userSensorToUpdate is null)
            {
                throw new ArgumentNullException("Error: Sensor not found!");
            }


            userSensorToUpdate.Name = name;
            userSensorToUpdate.Description = description;
            userSensorToUpdate.MinValue = minValue;
            userSensorToUpdate.MaxValue = maxValue;
            userSensorToUpdate.PollingInterval = pollingInterval;
            userSensorToUpdate.Latitude = latitude;
            userSensorToUpdate.Longitude = longitude;
            userSensorToUpdate.IsPublic = isPublic;
            userSensorToUpdate.Alarm = alarm;        

            this.context.UserSensors.Update(userSensorToUpdate);
            this.context.SaveChanges();
        }

        public async Task DeleteSensor(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid Id!");
            }

             var sensor = await this.context.UserSensors.
                FirstOrDefaultAsync(s => s.Id == id);

            if(sensor == null)
            {
                throw new ArgumentNullException($"Sensor with Id {id} does not exist!");
            }

            sensor.IsDeleted = true;

            this.context.UserSensors.Update(sensor);
            this.context.SaveChanges();
        }

        public async Task<Sensor> UpdateSensorValue(string apiSensorId)
        {
            if (string.IsNullOrEmpty(apiSensorId))
            {
                throw new ArgumentNullException("Sensor Id cannot be null!");
            }         

            return await this.context.Sensors.FirstOrDefaultAsync(s => s.IcbSensorId == apiSensorId);
        }
    }
}
