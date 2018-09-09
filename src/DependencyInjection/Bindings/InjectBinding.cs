using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Threading.Tasks;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    internal class InjectBinding : IBinding
    {
        private readonly Type _type;
        private readonly ServiceProviderHolder _serviceProviderHolder;

        internal InjectBinding(ServiceProviderHolder serviceProviderHolder, Type type)
        {
            _type = type;
            _serviceProviderHolder = serviceProviderHolder;
        }

        public bool FromAttribute => true;

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context) =>
            Task.FromResult((IValueProvider)new InjectValueProvider(value));

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            var value = _serviceProviderHolder.GetRequiredService(context.FunctionInstanceId, _type);
            return BindAsync(value, context.ValueContext);
        }

        public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();

        private class InjectValueProvider : IValueProvider
        {
            private readonly object _value;

            public InjectValueProvider(object value) => _value = value;

            public Type Type => _value.GetType();

            public Task<object> GetValueAsync() => Task.FromResult(_value);

            public string ToInvokeString() => _value.ToString();
        }
    }
}
