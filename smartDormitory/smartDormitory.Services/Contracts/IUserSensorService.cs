using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Services.Contracts
{
    public interface IUserSensorService
    {
        void AddSensor(string userId, int sensorId, double minValue, double maxValue, int pollingInterval, double latitude, double longitude, bool isPublic, bool alarm);
    }
}
