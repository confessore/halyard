using Autofac;
using Halyard.Interfaces;
using Halyard.Services;
using Halyard.Services.Interfaces;
using Halyard.Structs;
using System;
using System.Threading.Tasks;

namespace Halyard.Abstractions
{
    /// <inheritdoc cref="IHost"/>
    public abstract class Host : IHost
    {
        readonly IContainer _container;

        /// <summary>
        /// constructs a host for services related to memory manipulation of other active processes
        /// </summary>
        /// <param name="logging">a <see cref="bool"/> that enables/disables logging</param>
        public Host(bool logging = false)
        {
            var containerBuilder = new ContainerBuilder();
            if (logging)
                containerBuilder.RegisterType<LoggingService>().AsSelf().AutoActivate();
            containerBuilder.RegisterType<InjectionService>().As<IInjectionService>();
            _container = containerBuilder.Build();
        }

        #region Injection

        /// <inheritdoc />
        public async Task<bool> InjectAsync(
            int pid,
            Entry entry)
        {
            var injectionService = await GetInjectionServiceAsync();
            return await injectionService.InjectAsync(
                pid,
                entry);
        }

        /// <inheritdoc />
        public async Task<bool> EjectAsync()
        {
            var injectionService = await GetInjectionServiceAsync();
            return await injectionService.EjectAsync();
        }

        /// <inheritdoc />
        public Task<IInjectionService> GetInjectionServiceAsync()
        {
            using var scope = _container.BeginLifetimeScope();
            return Task.FromResult(scope.Resolve<IInjectionService>());
        }

        #endregion

        #region IDisposable

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (!disposed)
                disposed = true;
            await _container.DisposeAsync();
        }

        ~Host()
        {
            Dispose(false);
        }

        #endregion
    }
}
