# Dependency Injection Extensions for Azure Functions v2
This repo contains binding extensions for dependency injection in Azure Function v2. Out of the box  `Microsoft.Extensions.DependencyInjection` is used for dependency injection, but it is possible to use any IoC container that implements the `IServiceProvider` interface (for example Autofac).

## How to configure
The dependency injection bindings are available as a nuget package. Once the package is added to function project, a `WebJobsStartup` is needed to register and configure the dependency injection bindings.

This is an example `WebJobsStartup` class

```
using ExampleFunction;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ExampleFunction
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder) =>
            builder.AddDependencyInjection(ConfigureServices);

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITransientGreeter, Greeter>();
            services.AddScoped<IScopedGreeter, Greeter>();
            services.AddSingleton<ISingletonGreeter, Greeter>();
        }
    }

}
```
The nuget package contains two extension methods to register the dependency injection extensions.

```c#
AddDependencyInjection(this IWebJobsBuilder builder, Action<IServiceCollection> configureServices)
```
This configures the extension using the `Microsoft.Extensions.DependencyInjection` container. Services can be registered in the `configureServices` action.

```c#
AddDependencyInjection<TServiceProviderBuilder>(this IWebJobsBuilder builder) where TServiceProviderBuilder : IServiceProviderBuilder
```
This configures the extension to use what ever IoC container is returned from the `Build` method of the `IServiceProviderBuilder` implementation. It also gives access to other components, e.g. the configuration.

Example that uses Autofac
```c#
public class AutofacServiceProviderBuilder : IServiceProviderBuilder
{
    private readonly IConfiguration _configuration;

    public AutofacServiceProviderBuilder(IConfiguration configuration) => _configuration = configuration;

    public IServiceProvider Build()
    {
        Debug.WriteLine(_configuration["Setting"]); // Get a setting from the configuration.

        var services = new ServiceCollection();
        services.AddTransient<ITransientGreeter, Greeter>();
        services.AddScoped<IScopedGreeter, Greeter>();
        services.AddSingleton<ISingletonGreeter, Greeter>();

        var builder = new ContainerBuilder();
        builder.Populate(services); // Populate is needed to have support for scopes.

        return new AutofacServiceProvider(builder.Build());
    }
}
```

## Using the extension
Once the extension is registered and configured dependencies can be injected using the `Inject` attribute on a function. 

Example
```c#
[FunctionName("Greeter")]
public static IActionResult Run(
    [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req,
    [Inject]ITransientGreeter transientGreeter,
    [Inject]IScopedGreeter scopedGreeter,
    [Inject]ISingletonGreeter singletonGreeter,
    ILogger logger)
{
    logger.LogInformation("C# HTTP trigger function processed a request.");

    var result = String.Join(Environment.NewLine, new[] {
        $"Transient: {transientGreeter.Greet()}",
        $"Scoped: {scopedGreeter.Greet()}",
        $"Singleton: {singletonGreeter.Greet()}",
    });
    return new OkObjectResult(result);
}
```