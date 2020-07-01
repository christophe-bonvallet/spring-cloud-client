using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Formation.SpringCloud.Client.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using Steeltoe.Discovery.Client;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Refresh;

namespace Formation.SpringCloud.Client
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
            services.AddControllersWithViews();
            services.AddHealthActuator(Configuration);
            services.AddCloudFoundryActuators(Configuration);
            services.AddRefreshActuator(Configuration);
            services.AddDiscoveryClient(Configuration);
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddHystrixCommand<GetWeatherForecastCommand>("WeatherForecastService", Configuration);
            services.AddMemoryCache();
            services.AddHystrixMetricsStream(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseHealthActuator();
            app.UseCloudFoundryActuators();
            app.UseRefreshActuator();

            // Use the Steeltoe Discovery Client service
            app.UseDiscoveryClient();
            // Add Hystrix Metrics context to pipeline
            app.UseHystrixRequestContext();

            // Start Hystrix metrics stream service
            app.UseHystrixMetricsStream();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
