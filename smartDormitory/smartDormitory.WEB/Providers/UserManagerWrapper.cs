using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace smartDormitory.WEB.Providers
{
    public class UserManagerWrapper<T> : IUserManager<T> where T : class
    {
        private UserManager<T> _userManager;

        public UserManagerWrapper(UserManager<T> _userManager)
        {
            this._userManager = _userManager;
        }

        public IQueryable<T> Users => _userManager.Users;
        public IList<IPasswordValidator<T>> PasswordValidators => _userManager.PasswordValidators;
        public UserManager<T> Instance => _userManager;

        public async Task<IdentityResult> AddPasswordAsync(T user, string password)
        {
            return await this._userManager.AddPasswordAsync(user, password);
        }
        public async Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await this._userManager.GetUserAsync(claimsPrincipal);
        }
        public async Task<string> GetUserIdAsync(T user)
        {
            return await this._userManager.GetUserIdAsync(user);
        }
        public async Task<IdentityResult> RemovePasswordAsync(T user)
        {
            return await this._userManager.RemovePasswordAsync(user);
        }
        public async Task<IdentityResult> SetLockoutEnabledAsync(T user, bool enabled)
        {
            return await this._userManager.SetLockoutEnabledAsync(user, enabled);
        }
        public async Task<IdentityResult> SetLockoutEndDateAsync(T user, DateTimeOffset? lockoutEnd)
        {
            return await this._userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        }
    }
}
