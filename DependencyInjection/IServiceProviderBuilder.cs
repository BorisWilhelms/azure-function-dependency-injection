using System;

namespace DependencyInjection
{
    /// <summary>
    /// Defines the interface of builder that creates instances of an <see cref="IServiceProvider"/>.
    /// </summary>
    public interface IServiceProviderBuilder
    {
        /// <summary>
        /// Creates an instance of an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <returns></returns>
        IServiceProvider BuildServiceProvider();
    }
}
