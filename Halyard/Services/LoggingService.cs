using Halyard.Statics;
using Serilog;
using Serilog.Events;

namespace Halyard.Services
{
    sealed class LoggingService
    {
        public LoggingService()
        {
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Logger(config =>
                {
                    config
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File(
                     path: Paths.LogFile,
                     rollingInterval: RollingInterval.Day);
                });
            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
