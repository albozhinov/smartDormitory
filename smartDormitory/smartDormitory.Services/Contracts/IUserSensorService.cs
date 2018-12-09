using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services.Contracts
{
    public interface IUserSensorService
    {
        Task<IEnumerable<double>> GetSensorsTypeMinMaxValues(string tag);

        void AddSensor(string userId, int sensorId, string name, string description, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm);

        Task<IEnumerable<UserSensors>> GetAllPublicUsersSensorsAsync();

        Task<IEnumerable<UserSensors>> GetAllPrivateUserSensorsAsync(string id);

        Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id, int page = 1, int pageSize = 10);
        Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id);

        Task<IEnumerable<UserSensors>> GetAllUsersSensorsAsync(string searchByName, string searchByTag, int page = 1, int pageSize = 10);

        int TotalContainingText(string searchText);

        int Total();

        int TotalByName(string textName);
    }
}
