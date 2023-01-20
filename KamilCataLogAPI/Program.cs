using KamilCataLogAPI.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace KamilCataLogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = LoggingConfiguration.GetSerilogger();
           CreateHostBuilder(args).Build().Run();

           

        }

        /// <summary>
        /// WebHost builder.
        /// serilog logging provider instance attached
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                             
                });
    }
}
