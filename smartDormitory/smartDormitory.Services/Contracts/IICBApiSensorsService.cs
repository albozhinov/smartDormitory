using smartDormitory.Data;
using smartDormitory.Data.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace smartDormitory.Services.Contracts
{
    public interface IICBApiSensorsService
    {
        Task<IEnumerable<SensorDTOModel>> GetApiSensorsAsync();
    }
}