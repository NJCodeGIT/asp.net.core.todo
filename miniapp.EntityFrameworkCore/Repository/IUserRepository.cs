using miniapp.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.EntityFrameworkCore.Repository
{
    public interface IUserRepository
    {
        Task<bool> ValidateCredentials(string username, string password, bool remember, bool lockout);
        Task<bool> AddUser(AppUser appUser, string password);
        Task SignOut();
        Task<bool> AddExternalUser(Microsoft.AspNetCore.Http.HttpContext httpContext, AppUser appUser, bool persistentEnable);
        AuthenticationProperties GetAuthenticationProperties(string provider, string redirectUrl);
    }
}
