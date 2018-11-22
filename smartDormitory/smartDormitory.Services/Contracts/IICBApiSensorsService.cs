using smartDormitory.Data;
using smartDormitory.Data.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace smartDormitory.Services.Contracts
{
    public interface IICBApiSensorsService
    {
        Task<IEnumerable<SensorDTOModel>> GetApiSensorsAsync();

        Task UpdateSensorsAsync();

        IEnumerable<Sensor> ListAllSensors(int page = 1, int pageSize = 10);

        IEnumerable<Sensor> ListAllSensors();

        IEnumerable<Sensor> ListByContainingText(string searchText, int page = 1, int pageSize = 10);

        int TotalContainingText(string searchText);

        int Total();
    }
}