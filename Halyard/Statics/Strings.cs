using System.Reflection;

namespace Halyard.Statics
{
    static class Strings
    {
        public static string ExecutingAssemblyName =>
            Assembly.GetExecutingAssembly().GetName().Name;
    }
}
