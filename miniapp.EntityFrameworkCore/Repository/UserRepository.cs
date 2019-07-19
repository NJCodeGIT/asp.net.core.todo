using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using miniapp.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace miniapp.EntityFrameworkCore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> logger;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public UserRepository(ILogger<UserRepository> logger, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<bool> AddUser(AppUser appUser, string password)
        {
            try
            {
                this.logger.LogInformation("AddUser invoked");

                AppUser user = await this.userManager.FindByEmailAsync(appUser.Email);

                if (user == null)
                {
                    user = new AppUser()
                    {
                        FirstName = appUser.FirstName,
                        LastName = appUser.LastName,
                        Email = appUser.Email,
                        UserName = appUser.Email
                    };

                    var result = await this.userManager.CreateAsync(user, password);
                    return result.Succeeded;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"AddUser method failed: {ex}");
                return false;
            }
        }

        public async Task<bool> ValidateCredentials(string username, string password, bool remember, bool lockout)
        {
            try
            {
                this.logger.LogInformation("ValidateCredentials invoked");

                var result = await this.signInManager.PasswordSignInAsync(username, password, remember, lockout);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"ValidateCredentials method failed: {ex}");
                return false;
            }
        }

        public async Task SignOut()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<bool> AddExternalUser(Microsoft.AspNetCore.Http.HttpContext httpContext, AppUser appUser, bool persistentEnable)
        {
            try
            {

                var authResult1 = await httpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
                var authResult2 = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
                var authResult3 = await httpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                this.logger.LogInformation("AddExternalUser invoked");

                var info = await this.signInManager.GetExternalLoginInfoAsync();

                var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

                if (!result.Succeeded) //user does not exist yet
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    var newUser = new AppUser
                    {
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        EmailConfirmed = true
                    };
                    var createResult = await this.userManager.CreateAsync(newUser);
                    if (!createResult.Succeeded)
                        throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

                    await this.userManager.AddLoginAsync(newUser, info);
                    var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
                    await this.userManager.AddClaimsAsync(newUser, newUserClaims);
                    await this.signInManager.SignInAsync(newUser, isPersistent: false);

                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"AddExternalUser method failed: {ex}");
                return false;
            }
        }

        public AuthenticationProperties GetAuthenticationProperties(string provider, string redirectUrl)
        {
            try
            {
                this.logger.LogInformation("GetAuthenticationProperties invoked");

                var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return properties;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"GetAuthenticationProperties method failed: {ex}");
                return null;
            }
        }

    }
}
