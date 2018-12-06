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
                                    .Where(u => u.UserName.ToLower().Contains(searchText.ToLower(), StringComparison.InvariantCulture))
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }
        public async Task<int> GetTotalUserAsync(string searchText)
        {
            return await this.context.Users
                                     .Where(u => u.UserName.ToLower()
                                        .Contains(searchText.ToLower(), StringComparison.InvariantCulture))
                                     .CountAsync();
        }        
    }
}

