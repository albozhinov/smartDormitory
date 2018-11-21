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
        public IQueryable<T> Users => throw new NotImplementedException();

        public IList<IPasswordValidator<T>> PasswordValidators => throw new NotImplementedException();

        public UserManager<T> Instance => throw new NotImplementedException();

        public Task<IdentityResult> AddPasswordAsync(T user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> RemovePasswordAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SetLockoutEnabledAsync(T user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> SetLockoutEndDateAsync(T usrer, DateTimeOffset? lockoutEnd)
        {
            throw new NotImplementedException();
        }
    }
}
