using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Users.Infrastructure;
using Users.Models;

namespace Users
{
    public class Startup
    {

        IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;


                    options.Cookies.ApplicationCookie.Events =
                        new CookieAuthenticationEvents
                        {
                            OnRedirectToLogin = context =>
                            {
                                if (context.Request.Path.StartsWithSegments("/api") &&
                                    context.Response.StatusCode == (int) HttpStatusCode.OK)
                                {
                                    
                                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                                    context.Response.ContentType = "application/json";
                                    context.Response.WriteAsync("\"What did you say?\"");
                                }
                                else
                                    context.Response.Redirect(context.RedirectUri);

                                return Task.CompletedTask;
                            }
                        };




                    //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                    //options.Cookies.ApplicationCookie.LoginPath = "/Users/Login";

                }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddMvc();
        }

        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //core
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvcWithDefaultRoute();
        }
    }
}
