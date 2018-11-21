using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smartDormitory.Services
{
    public class MeasureTypesService : IMeasureTypesService
    {
        private readonly smartDormitoryDbContext context;

        public MeasureTypesService(smartDormitoryDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public MeasureType AddMeasureType(string type)
        {
            var sensorType = this.context.MeasureTypes
                    .FirstOrDefault(st => st.Type == type);

            if (sensorType != null)
            {
                throw new ArgumentException("Sensor type already exists!");
            }

            sensorType = new MeasureType
            {
                Type = type
            };

            this.context.MeasureTypes.Add(sensorType);
            this.context.SaveChanges();

            return sensorType;
        }
    }
}
