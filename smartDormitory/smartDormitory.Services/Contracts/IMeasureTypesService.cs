using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartDormitory.Services.Contracts
{
    public interface IMeasureTypesService
    {
        MeasureType AddMeasureType(string type);
    }
}
