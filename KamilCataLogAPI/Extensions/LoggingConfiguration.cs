using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KamilCataLogAPI.Extensions
{
    public static class LoggingConfiguration
    {
        public static ILogger GetSerilogger()
        {
           return  new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        }
    }
}
