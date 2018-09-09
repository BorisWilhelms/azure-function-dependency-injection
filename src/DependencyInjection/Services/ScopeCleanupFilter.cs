using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using System.Threading.Tasks;

namespace Willezone.Azure.WebJobs.Extensions.DependencyInjection
{
    internal class ScopeCleanupFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly ServiceProviderHolder _scopeHolder;

        public ScopeCleanupFilter(ServiceProviderHolder scopeHolder) =>
            _scopeHolder = scopeHolder;

        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            _scopeHolder.RemoveScope(exceptionContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            _scopeHolder.RemoveScope(executedContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
