using miniapp.EntityFrameworkCore.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.EntityFrameworkCore.Context
{
    public class EntitySeeder
    {
        private readonly EntityContext entityContext;
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<AppUser> userManager;

        public EntitySeeder(EntityContext entityContext, IHostingEnvironment hosting,
            UserManager<AppUser> userManager)
        {
            this.entityContext = entityContext;
            this.hosting = hosting;
            this.userManager = userManager;
        }

        public async Task SeedAsync()
        {
            this.entityContext.Database.EnsureCreated();

            AppUser appUser = await this.userManager.FindByEmailAsync("niju.mn@live.com");

            if (appUser == null)
            {
                appUser = new AppUser()
                {
                    FirstName = "Niju",
                    LastName = "Nizar",
                    Email = "niju.mn@live.com",
                    UserName = "niju.mn@live.com"
                };

                var result =await this.userManager.CreateAsync(appUser, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seed");
                }
            }

            if (!this.entityContext.Menus.Any())
            {
                var menuFilePath = Path.Combine(this.hosting.ContentRootPath, "menu.json");
                var menuJson = File.ReadAllText(menuFilePath);
                var menus = JsonConvert.DeserializeObject<IEnumerable<Menu>>(menuJson);

                this.entityContext.Menus.AddRange(menus);
                this.entityContext.SaveChanges();
            }
        }
    }
}
