using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace DependencyInjection
{
    internal class InjectBindingProvider : IBindingProvider
    {
        public static readonly ConcurrentDictionary<Guid, IServiceScope> Scopes =
            new ConcurrentDictionary<Guid, IServiceScope>();

        private IServiceProvider _serviceProvider;

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (_serviceProvider == null)
            {
                _serviceProvider = CreateServiceProvider(context.Parameter.Member.DeclaringType.Assembly);
            }

            IBinding binding = new InjectBinding(_serviceProvider, context.Parameter.ParameterType);
            return Task.FromResult(binding);
        }

        private IServiceProvider CreateServiceProvider(Assembly assembly)
        {
            var builder = ServiceProviderBuilderHelper.GetBuilder(assembly);
            return builder.BuildServiceProvider();
        }
    }
}
