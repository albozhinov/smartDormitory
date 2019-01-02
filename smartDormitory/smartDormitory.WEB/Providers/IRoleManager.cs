using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Providers
{
    public interface IRoleManager<T> where T: class
    {
        Task<bool> RoleExistsAsync(string roleName);

        IQueryable<T> Roles { get; }

        RoleManager<T> Istance { get; }
    }
}
