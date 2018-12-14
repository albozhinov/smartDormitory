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
                                    .Where(u => u.UserName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }
        public async Task<int> GetTotalUserAsync(string searchText)
        {
            return await this.context.Users
                                     .Where(u => u.UserName
                                        .Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                     .CountAsync();
        }     
        
        public async Task<string> GetUserEmailAsync(string id)
        {
            return await this.context.Users
                .Where(u => u.Id == id)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> GetUserReceiveEmailsAsync(string id)
        {
            return await this.context.Users
                .Where(u => u.Id == id)
                .Select(u => u.ReceiveEmails)
                .FirstOrDefaultAsync();
        }
    }
}

