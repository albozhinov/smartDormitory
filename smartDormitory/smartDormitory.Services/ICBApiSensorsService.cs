using smartDormitory.Data.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using smartDormitory.Services.Contracts;
using smartDormitory.Data;
using smartDormitory.Data.DTOModels;

namespace smartDormitory.Services
{
    public class ICBApiSensorsService : IICBApiSensorsService
    {
        private readonly smartDormitoryDbContext context;

        public ICBApiSensorsService(smartDormitoryDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<SensorDTOModel>> GetApiSensorsAsync()
        {

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://telerikacademy.icb.bg/");
            client.DefaultRequestHeaders.Add("auth-token", "8e4c46fe-5e1d-4382-b7fc-19541f7bf3b0");
            var response = await client.GetAsync($"api/sensor/all");
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();
            var sensors = JsonConvert.DeserializeObject<List<SensorDTOModel>>(stringResult);

            foreach (var sensor in sensors)
            {
                await this.GetApiSensorByIdAsync(sensor, sensor.SensorId);
            }

            return sensors;

        }

        public async Task<SensorDTOModel> GetApiSensorByIdAsync(SensorDTOModel sensor, string id)
        {

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://telerikacademy.icb.bg/");
            client.DefaultRequestHeaders.Add("auth-token", "8e4c46fe-5e1d-4382-b7fc-19541f7bf3b0");
            var response = await client.GetAsync($"api/sensor/{id}");
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();
            var sen = JsonConvert.DeserializeObject<SensorDTOModel>(stringResult);

            sensor.TimeStamp = sen.TimeStamp;
            sensor.ValueType = sen.ValueType;
            sensor.Value = sen.Value;


            return sensor;

        }




    }
}
