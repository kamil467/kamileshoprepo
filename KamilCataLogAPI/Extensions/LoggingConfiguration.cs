using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

namespace KamilCataLogAPI.Extensions
{
    public static class LoggingConfiguration
    {
        public static ILogger GetSerilogger()
        {
            var logPath = Path.Combine(@"C:\Temp", @"Logs\log.txt");
            return  new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(logPath,     // sink - to record log events in external representation, '
                                         // we use File Sink provider here.
                                         // sinks usually provided via nuget as external service.
                                         // serilog.Sinks.File
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true
               )
            .CreateLogger();
        }
    }
}
