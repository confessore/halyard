using Halyard.Structs;
using System.Threading.Tasks;

namespace Halyard.Services.Interfaces
{
    /// <summary>
    /// a service for injecting libaries into active processes
    /// </summary>
    public interface IInjectionService
    {
        /// <summary>
        /// injects a library into an active process
        /// </summary>
        /// <param name="pid">an <see cref="int"/> process id</param>
        /// <param name="config">a <see cref="string"/> runtimeconfig.json filename</param>
        /// <param name="lib">a <see cref="string"/> .dll filename</param>
        /// <param name="type">a <see cref="string"/> namespace qualified object name</param>
        /// <param name="method">a <see cref="string"/> method name</param>
        /// <param name="@delegate">a <see cref="string"/> delegate name</param>
        /// <example>
        /// shows how to call <see cref="InjectAsync(int, string, string, string, string, string)" />
        /// <code>
        /// var injected = await InjectAsync(
        ///     pid: 0,
        ///     config: "Halyard.runtimeconfig.json",
        ///     lib: "Halyard.dll",
        ///     type: "Halyard.EntryClass, Halyard",
        ///     method: "EntryMethod",
        ///     @delegate: "Halyard.EntryClass+EntryDelegate, Halyard");
        /// </code>
        /// </example>
        /// <returns>a <see cref="bool"/> from a <see cref="Task"/> that is true if the library was successfully injected</returns>
        Task<bool> InjectAsync(
            int pid,
            Entry entry);

        /// <summary>
        /// ejects a library from an active process
        /// </summary>
        /// <returns>a <see cref="bool"/> from a <see cref="Task"/> that is true if the library was successfully ejected</returns>
        Task<bool> EjectAsync();
    }
}
