using System.IO;

namespace Halyard.Statics
{
    static class Paths
    {
        public static string CurrentDirectory =>
            Directory.GetCurrentDirectory();

        public static string LogsDirectory =>
            Path.Combine(CurrentDirectory, "logs");

        public static string LogFile =>
            Path.Combine(LogsDirectory, Logs.ExecutingAssembly);
    }
}
