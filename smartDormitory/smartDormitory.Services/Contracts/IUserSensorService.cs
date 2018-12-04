using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services.Contracts
{
    public interface IUserSensorService
    {
        IEnumerable<UserSensors> GetAllUserSensors(int sensorId, string userId);

        void AddSensor(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm);

        Task<IEnumerable<UserSensors>> GetAllPublicUsersSensorsAsync();
    }
}
