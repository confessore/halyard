using Halyard.Services.Interfaces;
using Halyard.Structs;
using System;
using System.Threading.Tasks;

namespace Halyard.Interfaces
{
    /// <summary>
    /// an object for hosting services related to memory manipulation of other active processes
    /// </summary>
    public interface IHost : IDisposable
    {
        /// <inheritdoc cref="IInjectionService.InjectAsync(int, Entry)" />"
        Task<bool> InjectAsync(
            int pid,
            Entry entry);

        /// <inheritdoc cref="IInjectionService.EjectAsync" />
        Task<bool> EjectAsync();

        /// <summary>
        /// gets an injection service interface
        /// </summary>
        /// <returns>an <see cref="IInjectionService"/> from a Task</returns>
        Task<IInjectionService> GetInjectionServiceAsync();
    }
}
