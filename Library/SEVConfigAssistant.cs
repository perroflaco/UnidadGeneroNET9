using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Collections.Generic;

namespace SEV.Library
{
    public class SEVConfigAssistant
    {
        public static IConfiguration Configuration { get; set; }
        private IConfiguration CurrentConfiguration { get; set; }
        private HttpConfiguration CurrentHTTPConfiguration { get; set; }
        private DefaultFilesOptions CurrentDefaultHome { get; set; }
        private IServiceCollection CurrentServices { get; set; }
        private IApplicationBuilder CurrentApp { get; set; }

        private Boolean MustEnableHALReadFiles { get; set; }

        private List<String> ConfigurationFilesToLoad {get; set;}

        private IWebHostEnvironment Enviroment { get; set; }

        private String SEVCORSPolicy = "SEVCORSPolicy";

        public SEVConfigAssistant(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            Configuration = configuration;
            CurrentConfiguration = Configuration;
            Enviroment = enviroment;
            MustEnableHALReadFiles = false;
            CurrentHTTPConfiguration = new HttpConfiguration();
            CurrentDefaultHome = new DefaultFilesOptions();
            ConfigurationFilesToLoad = new List<String>();
        }

        public SEVConfigAssistant ServicesSettingsFrom(IServiceCollection services)
        {
            CurrentServices = services;
            return this;
        }

        public SEVConfigAssistant AppSettingsFrom (IApplicationBuilder app)
        {
            CurrentApp = app;
            return this;
        }

        // Config Section
        public SEVConfigAssistant EnableReloadConfigOnChange(Boolean Enable = true)
        {
            if (Enable)
            {
                ReloadConfigOnChange reloader = new ReloadConfigOnChange(Enviroment, MustEnableHALReadFiles, ConfigurationFilesToLoad);
                Configuration = reloader.Configuration;
                if (MustEnableHALReadFiles)
                {
                    HALConfigReader.SetConfiguration(Configuration);
                }
            }
            else
            {
                Configuration = CurrentConfiguration;
            };
            return this;
        }

        public SEVConfigAssistant EnableHALConfig()
        {
            MustEnableHALReadFiles = true;
            return this;
        }

        public IConfiguration GetCurrentConfig()
        {
            return Configuration;
        }

        // Extra Configuration Files Section
        public SEVConfigAssistant AddJSONConfigFile(String LocalPath)
        {
            ConfigurationFilesToLoad.Add(LocalPath);
            return this;
        }

        // JWT Section
        public SEVConfigAssistant EnableJWT(String Profile = "")
        {
            Profile = Profile.Length == 0 ? "JWT" : "JWT:" + Profile;
            CurrentServices.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer   = Configuration[ Profile + ":Issuer"  ],
                    ValidAudience = Configuration[ Profile + ":Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                                                            (
                                                                Encoding
                                                                    .UTF8
                                                                        .GetBytes
                                                                            (
                                                                                Configuration[ Profile + ":SecretKey" ]
                                                                            )
                                                            )
                };
            });
            return this;
        }

        public SEVConfigAssistant ActivateJWTAuthorization()
        {
            CurrentApp.UseAuthentication();
            return this;
        }

        // Pascal Sintax Section

        public SEVConfigAssistant EnablePascalSintaxOnResponse()
        {   
            CurrentHTTPConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            CurrentHTTPConfiguration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            // Net core 3.+
            CurrentServices.AddControllers()
                .AddNewtonsoftJson
                (
                    Options =>
                    {
                        Options.SerializerSettings.ContractResolver = new DefaultContractResolver(); // .JsonSerializerOptions.PropertyNamingPolicy = null;
                    }
                );

            /*
             * 
             * NetCore 2.2 
            CurrentServices.AddMvc().AddJsonOptions
            (
                Options => Options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            );*/
            return this;
        }

        public HttpConfiguration GetCurrentHttpConfig()
        {
            return CurrentHTTPConfiguration;
        }

        // CORS Section
        public SEVConfigAssistant EnableCors()
        {
            //CurrentHTTPConfiguration.EnableCors();
            CurrentServices.AddCors(
                Options => Options.AddPolicy(
                    SEVCORSPolicy,
                    Builder => ApplyCorsPolicy(Builder)
                )
            );
            return this;
        }

        public SEVConfigAssistant ActivateCORS()
        {
            CurrentApp.UseCors(SEVCORSPolicy);
            return this;
        }



        public CorsPolicyBuilder ApplyCorsPolicy (CorsPolicyBuilder builder)
        {
            String AllowedOrigins = Configuration["CORS:Origins"];
            string[] OriginsToProcess = AllowedOrigins
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.TrimEnd('/'))
                            .ToArray();
            foreach (string OriginToProcess in OriginsToProcess)
            {
                builder.
                        WithOrigins(
                            OriginToProcess
                        ).
                        SetIsOriginAllowedToAllowWildcardSubdomains().
                        SetIsOriginAllowed((host) => true); // For NC-3.1 Compatibility
            }
            builder.
                    
                    AllowAnyMethod().
                    AllowCredentials().
                    AllowAnyHeader();
            return builder;
        }

        // Default HomePage Section
        public SEVConfigAssistant EnableDefaultHome(String FileName = "index.html")
        {
            CurrentDefaultHome.DefaultFileNames.Clear();
            CurrentDefaultHome.DefaultFileNames.Add(FileName);
            CurrentApp.UseDefaultFiles(GetCurrentDefaultHome());
            CurrentApp.UseStaticFiles();
            return this;
        }

        public DefaultFilesOptions GetCurrentDefaultHome()
        {
            return CurrentDefaultHome;
        }
    }
}
