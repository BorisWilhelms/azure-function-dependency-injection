using Microsoft.Azure.WebJobs.Description;
using System;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    /// <summary>
    /// Attribute used to inject a dependency into the function completes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    [Binding]
    public sealed class InjectAttribute : Attribute
    {
    }
}
