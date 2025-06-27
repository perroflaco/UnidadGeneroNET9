using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SEV.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnidadGenero
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public SEVConfigAssistant SEVConfig { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            SEVConfig = new SEVConfigAssistant(configuration, environment);
            SEVConfig.EnableReloadConfigOnChange();
            Configuration = SEVConfig.GetCurrentConfig();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            SEVConfig.
                ServicesSettingsFrom(services).
                    EnablePascalSintaxOnResponse().
                    EnableCors().
                    EnableJWT("Data");

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();        
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            SEVConfig.
                AppSettingsFrom(app).
                    ActivateCORS().
                    EnableDefaultHome("index.html").
                    ActivateJWTAuthorization();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapControllers();
            });
        }

    }
}
