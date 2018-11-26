using smartDormitory.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services.Contracts
{
    public interface ISensorService
    {
        Task<IEnumerable<Sensor>> GetSensorsAsync();

    }
}
