using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlApp.Controllers;
using FlightControlApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightControlApp
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
            services.AddControllers();
            string IP = Configuration.GetValue<string>("Logging:SimulatorInfo:IP");
            int telnetPort = Configuration.GetValue<int>("Logging:SimulatorInfo:TelnetPort");
            IModel commandManager = new SimulatorModel(telnetPort, IP);
            // bind all model classes as singletons
            services.AddSingleton(commandManager);
            // tell framework to obtain Controller instances from ServiceProvider.
            services.AddMvc().AddControllersAsServices();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
