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
            services.AddControllersWithViews();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IServicePasswordRepository, ServicePasswordRepository>();
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<ILoginAttemptRepository, LoginAttemptRepository>();
            services.AddTransient<IBlockingRepository, BlockingRepository>();

            services.AddTransient<ISecretsService, PrimitiveSecretsService>();
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
