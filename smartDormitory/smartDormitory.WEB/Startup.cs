using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using smartDormitory.Data.Context;
using smartDormitory.Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using smartDormitory.WEB.Areas.Identity.Services;
using smartDormitory.Services;
using smartDormitory.Services.Contracts;
using smartDormitory.WEB.Providers;

namespace smartDormitory.WEB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<smartDormitoryDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<smartDormitoryDbContext>()
                .AddDefaultTokenProviders();            

            //Disable some password options to test easily!
            services.Configure<IdentityOptions>(options =>
           {
               options.Password.RequireDigit = false;
               options.Password.RequiredLength = 3;
               options.Password.RequireLowercase = false;
               options.Password.RequireNonAlphanumeric = false;
               options.Password.RequireUppercase = false;
           });

            
            services.AddScoped<IICBApiSensorsService, ICBApiSensorsService>();
            services.AddScoped<IMeasureTypesService, MeasureTypesService>();
            services.AddScoped(typeof(IUserManager<>), typeof(UserManagerWrapper<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddScoped<IUserSensorService, UserSensorService>();


            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
             .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
         .AddRazorPagesOptions(options =>
         {
             options.AllowAreas = true;
             options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
             options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
         });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "adminArea",
                    template: "{area:exists}/{controller=Users}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(service).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            //Adding Admin Role 
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database 
                await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }

            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                await RoleManager.CreateAsync(new IdentityRole("User"));
            }
            //Assign Admin role to the main User here we have given our newly registered  
            //login id for Admin management 
            //User user = await UserManager.FindByEmailAsync("gosho@abv.bg");
            //var User = new User();
            //await UserManager.AddToRoleAsync(user, "Admin");
        }
    }
}
