namespace Halyard.Statics
{
    static class Logs
    {
        static string Extension =>
            ".log";

        public static string ExecutingAssembly =>
            string.Concat(Strings.ExecutingAssemblyName, Extension);
    }
}
