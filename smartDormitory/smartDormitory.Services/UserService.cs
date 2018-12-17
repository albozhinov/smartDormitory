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

        public UserService(smartDormitoryDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<User>> GetUsersAsync(string searchText, int page = 1, int pageSize = 3)
        {
            if(searchText == null)
            {
                throw new ArgumentNullException("Search text cannot be null!");
            }

            if (page < 1)
            {
                throw new ArgumentException("Page cannot be less than 1!");
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("Page cannot be less than 1!");
            }

            return await this.context
                                    .Users
                                    .Where(u => u.UserName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
        }
        public async Task<int> GetTotalUserAsync(string searchText)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException("Search text cannot be null!");
            }

            return await this.context.Users
                                     .Where(u => u.UserName
                                        .Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                                     .CountAsync();
        }     
        
        public async Task<string> GetUserEmailAsync(string id)
        {
            if(id == null)
            {
                throw new ArgumentNullException("Id cannot be null!");
            }

            return await this.context.Users
                .Where(u => u.Id == id)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();
        }
    }
}

