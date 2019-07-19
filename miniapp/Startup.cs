using AutoMapper;
using miniapp.EntityFrameworkCore.Context;
using miniapp.EntityFrameworkCore.Entities;
using miniapp.EntityFrameworkCore.Repository;
using miniapp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using miniapp.ApiControllers;

namespace miniapp
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentity<AppUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<EntityContext>().AddDefaultTokenProviders(); ;

            services.AddDbContext<EntityContext>(cfg =>
            {
                cfg.UseSqlServer(this.configuration.GetConnectionString("EntityContextConnectionString"));
            });


            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = this.configuration["FacebookAccountDetails:AppId"];
                    options.AppSecret = this.configuration["FacebookAccountDetails:AppSecret"].ToString();
                })
                .AddTwitter(options =>
                {
                    options.ConsumerKey = this.configuration["TwitterAccountDetails:AppId"];
                    options.ConsumerSecret = this.configuration["TwitterAccountDetails:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = this.configuration["GoogleAccountDetails:AppId"];
                    options.ClientSecret = this.configuration["GoogleAccountDetails:AppSecret"];
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                });

#pragma warning disable CS0618 // Type or member is obsolete
            services.AddAutoMapper();
#pragma warning restore CS0618 // Type or member is obsolete

            services.AddTransient<IMailService, NullMailService>();
            services.AddTransient<EntitySeeder>();
            AddRepositoryToServices(services);

            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            }).AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;


        }

        private static void AddRepositoryToServices(IServiceCollection services)
        {
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGenericRepository<ToDo>, GenericRepository<ToDo>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttps(301, 44318));
            app.UseNodeModules(env);
            app.UseDefaultFiles();
            app.UseStaticFiles();



            app.UseAuthentication();
            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "app", Action = "Index" });
            });
        }
    }
}
