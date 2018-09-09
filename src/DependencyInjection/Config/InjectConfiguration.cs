using Microsoft.Azure.WebJobs.Host.Config;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    internal class InjectConfiguration : IExtensionConfigProvider
    {
        public readonly InjectBindingProvider _injectBindingProvider;

        public InjectConfiguration(InjectBindingProvider injectBindingProvider) =>
            _injectBindingProvider = injectBindingProvider;

        public void Initialize(ExtensionConfigContext context) => context
                .AddBindingRule<InjectAttribute>()
                .Bind(_injectBindingProvider);
    }
}

