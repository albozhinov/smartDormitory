using Microsoft.EntityFrameworkCore;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using smartDormitory.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.Services
{
    public class UserService : IUserService
    {
        private smartDormitoryDbContext context;

        public UserService(smartDormitoryDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<ICollection<User>> GetUsersAsync(string searchText, int page = 1, int pageSize = 3)
        {
            return await this.context
                                    .Users
                                    .Where(u => u.UserName.Contains(searchText, StringComparison.InvariantCulture))                                    
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();            
        }
        public async Task<int> GetTotalUserAsync(string searchText)
        {
            return await this.context.Users
                                     .Where(u => u.UserName
                                        .Contains(searchText, StringComparison.InvariantCulture))
                                     .CountAsync();
        }

        public async Task<IEnumerable<UserSensors>> GetAllUserSensorsByContainingTagAsync(string id, string searchText, int page = 1, int pageSize = 10)
        {
            return await this.context.UserSensors
                .Where(us => us.UserId == id)
                .Where(us => us.Sensor.Tag.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .Include(s => s.Sensor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSensors>> GetAllUserSensorsAsync(string id, int page = 1, int pageSize = 10)
        {
            return await this.context.UserSensors
                .Where(us => us.UserId == id)
                .Include(s => s.Sensor)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public int Total()
        {
            return this.context.UserSensors.Count();
        }

        public int TotalContainingText(string searchText)
        {
            return this.context.UserSensors
                .Where(s => s.Sensor.Tag.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .Count();
        }
    }
}

