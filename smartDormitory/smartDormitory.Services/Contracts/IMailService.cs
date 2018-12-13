using smartDormitory.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartDormitory.Services.Contracts
{
    public interface IMailService
    {
        Task SendEmail(IEnumerable<UserSensors> userSensors, string username, string email);
    }
}
