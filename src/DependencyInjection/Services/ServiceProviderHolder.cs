using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    internal class ServiceProviderHolder
    {
        private readonly ConcurrentDictionary<Guid, IServiceScope> _scopes = new ConcurrentDictionary<Guid, IServiceScope>();
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderHolder(IServiceProvider serviceProvider) =>
            _serviceProvider = serviceProvider ?? throw new InvalidOperationException("No service provider provided!");

        public void RemoveScope(Guid functionInstanceId)
        {
            if (_scopes.TryRemove(functionInstanceId, out var scope))
            {
                scope.Dispose();
            }
        }

        public object GetRequiredService(Guid functionInstanceId, Type serviceType)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
            if (scopeFactory == null)
            {
                throw new InvalidOperationException("The current service provider does not support scoping!");
            }

            var scope = _scopes.GetOrAdd(functionInstanceId, (_) => scopeFactory.CreateScope());
            return scope.ServiceProvider.GetRequiredService(serviceType);
        }
    }
}
