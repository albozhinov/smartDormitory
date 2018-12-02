using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartDormitory.WEB.Providers
{
    public class RoleManagerWrapper<T> : IRoleManager<T> where T : class
    {
        private RoleManager<T> _roleManager;

        public RoleManagerWrapper(RoleManager<T> roleManager)
        {
            this._roleManager = roleManager;
        }

        public IQueryable<T> Roles => _roleManager.Roles;
        public RoleManager<T> Istance => _roleManager;

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await this._roleManager.RoleExistsAsync(roleName);
        }
    }
}
