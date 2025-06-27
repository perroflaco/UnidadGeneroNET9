using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SEV.Library
{
    public class ReloadConfigOnChange

    {
        public IConfiguration Configuration { get; }
        public ReloadConfigOnChange(IWebHostEnvironment env, Boolean MustEnableHALReadFiles, List<String> AdditionalConfigFiles = null)
        {
            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            if (MustEnableHALReadFiles)
            {
                foreach
                (
                    var jsonFilename in Directory
                                            .EnumerateFiles
                                            (
                                                Directory.GetCurrentDirectory() +
                                                    "/HAL",
                                                "*.json",
                                                SearchOption.AllDirectories
                                                )
                )
                {
                    builder.AddJsonFile(jsonFilename, optional: true, reloadOnChange: true);
                }
            }
            if(AdditionalConfigFiles!=null)
            {
                AdditionalConfigFiles.ForEach
                (
                    CurrentFilePath => {
                        String JSONFileToAdd = Directory.GetCurrentDirectory() + "/" + CurrentFilePath;
                        if(File.Exists(JSONFileToAdd))
                        {
                            builder.AddJsonFile( JSONFileToAdd, optional: true, reloadOnChange: true );
                        }
                    }
                );
            }
            Configuration = builder.AddEnvironmentVariables().Build();
        }
    }
}
