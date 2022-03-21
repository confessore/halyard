using Serilog;
using Serilog.Events;
using System.Diagnostics;

namespace Halyard.Test
{
    public static class EntryClassTest
    {
        public delegate void EntryDelegateTest();
        public static void EntryMethodTest()
        {
#if DEBUG
            Debugger.Launch();
#endif
            //var loggerConfig = new LoggerConfiguration()
            //    .MinimumLevel.Verbose()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Logger(config =>
            //    {
            //        config
            //        .MinimumLevel.Information()
            //        .WriteTo.Console()
            //        .WriteTo.File(
            //        path: "injected.log",
            //        rollingInterval: RollingInterval.Day);
            //    });
            //Log.Logger = loggerConfig.CreateLogger();
            //Log.Information("hello world from injected mother trucker");
        }
    }
}
