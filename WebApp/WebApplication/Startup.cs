using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Implementation;
using Repository.Infrastructure;
using Services.Implementation;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Two services needed by Wangkanai Detection
            services.AddDetection();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAntiforgery();
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Home/Denied";
                });
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IServicePasswordRepository, ServicePasswordRepository>();
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<ILoginAttemptRepository, LoginAttemptRepository>();
            services.AddTransient<IBlockingRepository, BlockingRepository>();

            services.AddTransient<ISecretsService, PrimitiveSecretsService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IServicePasswordService, ServicePasswordService>();
            services.AddTransient<IDeviceService, DeviceService>();
            services.AddTransient<ILoginAttemptService, LoginAttemptService>();
            services.AddTransient<IBlockingService, BlockingService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDetection();
            app.UseRouting();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) => 
            {   context.Response.OnStarting(() => 
                {
                    context.Response.Headers.Add("Content-Security-Policy",
                        "default-src 'self';");
                    return Task.FromResult(0);
                });
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
