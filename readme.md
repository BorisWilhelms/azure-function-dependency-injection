# Dependency Injection Extensions for Azure Functions v2

- [Dependency Injection Extensions for Azure Functions v2](#dependency-injection-extensions-for-azure-functions-v2)
    - [About](#about)
    - [How to configure](#how-to-configure)
    - [Using the extension](#using-the-extension)
    - [Azure Deployment](#azure-deployment)

## About
This repo contains binding extensions for dependency injection in Azure Function v2. Out of the box  `Microsoft.Extensions.DependencyInjection` is used for dependency injection, but it is possible to use any IoC container that implements the `IServiceProvider` interface (for example Autofac).

## How to configure
The dependency injection bindings are available as a [nuget package](https://www.nuget.org/packages/Willezone.Azure.WebJobs.Extensions.DependencyInjection). Once the package is added to function project, a `WebJobsStartup` is needed to register and configure the dependency injection bindings.

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

## Azure Deployment
Currently there is an issue when publishing your function application that the required `extensions.json` is not created correctly. The issue is discussed [here](https://github.com/Azure/azure-functions-host/issues/3386#issuecomment-419565714). Luckily there is a workaround for this: Just copy the [Directory.Build.targets](tools/Directory.Build.targets) file into your Azure Functions project, this will then create the correct `extensions.json` file.