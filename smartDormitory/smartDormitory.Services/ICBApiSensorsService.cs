using smartDormitory.Data.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using smartDormitory.Services.Contracts;
using smartDormitory.Data;
using smartDormitory.Data.DTOModels;
using System.Text.RegularExpressions;
using System.Linq;
using smartDormitory.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace smartDormitory.Services
{
    public class ICBApiSensorsService : IICBApiSensorsService
    {
        private readonly smartDormitoryDbContext context;
        private readonly IMeasureTypesService measureTypesService;
        
        private const string ApiBaseAddress = "http://telerikacademy.icb.bg/";
        private const string ApiGetAll = "api/sensor/all";
        private const string ApiGetById = "api/sensor/";
        private const string ApiKey = "auth-token";
        private const string ApiValue = "8e4c46fe-5e1d-4382-b7fc-19541f7bf3b0";

        public ICBApiSensorsService(smartDormitoryDbContext context, IMeasureTypesService measureTypesService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.measureTypesService = measureTypesService ?? throw new ArgumentNullException(nameof(measureTypesService));
        }

        public async Task<IEnumerable<SensorDTOModel>> GetApiSensorsAsync()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Add(ApiKey, ApiValue);
            var response = await client.GetAsync(ApiGetAll);
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
            if (sensor == null)
            {
                throw new ArgumentNullException("Sensor cannot be null!");
            }

            if(id == null)
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            var client = new HttpClient();

            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Add(ApiKey, ApiValue);
            var response = await client.GetAsync($"{ApiGetById}{id}");
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();

            var sen = JsonConvert.DeserializeObject<SensorDTOModel>(stringResult);

            sensor.TimeStamp = sen.TimeStamp;
            sensor.ValueType = sen.ValueType;
            sensor.Value = sen.Value;

            return sensor;
        }

        public async Task UpdateSensorsAsync()
        {
            IEnumerable<SensorDTOModel> dtoSensors = await GetApiSensorsAsync();

            var sensors = new List<Sensor>();

            foreach (var sen in dtoSensors)
            {
                Sensor sensor = this.context.Sensors.FirstOrDefault(s => s.Guid == sen.SensorId);
                if ( sensor != null)
                {
                    sensor.Value = this.ValueParse(sen.Value);
                    this.context.Sensors.Update(sensor);
                    this.context.SaveChanges();
                    continue;
                }

                MeasureType measureType = this.context.MeasureTypes
                                                .FirstOrDefault(mt => mt.Type == sen.MeasureType);
                if ( measureType == null)
                {
                    measureType = this.measureTypesService.AddMeasureType(sen.MeasureType);
                }
                var minAndMaxValues = this.MinMaxValues(sen.Description);

                var senso = new Sensor()
                {
                    Guid = sen.SensorId,
                    Description = sen.Description,
                    TimeStamp = sen.TimeStamp,
                    Value = this.ValueParse(sen.Value),
                    Tag = sen.Tag,
                    PollingInterval = sen.MinPollingIntervalInSeconds,
                    MinValue = minAndMaxValues[0],
                    MaxValue = minAndMaxValues[1],
                    MeasureTypeId = measureType.Id,
                    Url = $"{ApiBaseAddress}{ApiGetById}{sen.SensorId}"
                };

                sensors.Add(senso);
            }

            foreach (var sen in sensors)
            {
                this.context.Sensors.Add(sen);
            }
            this.context.SaveChanges();
        }

        public IEnumerable<Sensor> ListAllSensors(int page = 1, int pageSize = 10)
        {
            return this.context.Sensors
                .Include(s => s.MeasureType)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Sensor> ListAllSensors()
        {
            return this.context.Sensors
                .Include(s => s.MeasureType)
                .ToList();
        }

        public int Total()
        {
            return this.context.Sensors.Count();
        }

        public IEnumerable<Sensor> ListByContainingText(string searchText, int page = 1, int pageSize = 10)
        {
            return this.context.Sensors
                .Include(s => s.MeasureType)
                .Where(s => s.Tag.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int TotalContainingText(string searchText)
        {
            return this.context.Sensors
                .Include(s => s.MeasureType)
                .Where(s => s.Tag.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .Count();
        }

        private double ValueParse(string value)
        {
            double val = 0;
            if (value == "true")
            {
                val = 1;
            }
            else if (value == "false")
            {
                val = 0;
            }
            else
            {
                val = double.Parse(value);
            }
            return val;
        }

        private List<double> MinMaxValues(string description)
        {
            if(description.Contains("true") || description.Contains("false"))
            {
                return new List<double>() { 0, 1 };
            }
            var minAndMaxValues = new List<double>();
            string[] numbers = Regex.Split(description, @"\D+");
            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    minAndMaxValues.Add(double.Parse(value));
                }
            }
            return minAndMaxValues;
        }

    }
}
