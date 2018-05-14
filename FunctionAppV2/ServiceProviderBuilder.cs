using DependencyInjection;
using Lib;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FunctionAppV2
{
    public class ServiceProviderBuilder : IServiceProviderBuilder
    {
        public IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddTransient<IGreeter, Lib.Greeter>();

            return services.BuildServiceProvider(true);
        }
    }
}
