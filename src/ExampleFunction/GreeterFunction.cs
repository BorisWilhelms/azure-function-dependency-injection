using ExampleFunction.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace ExampleFunction
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req,
            [Inject]ITransientGreeter transientGreeter1,
            [Inject]ITransientGreeter transientGreeter2,
            [Inject]IScopedGreeter scopedGreeter1,
            [Inject]IScopedGreeter scopedGreeter2,
            [Inject]ISingletonGreeter singletonGreeter1,
            [Inject]ISingletonGreeter singletonGreeter2,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = String.Join(Environment.NewLine, new[] {
                $"Transient: {transientGreeter1.Greet()}",
                $"Transient: {transientGreeter2.Greet()}",
                $"Scoped: {scopedGreeter1.Greet()}",
                $"Scoped: {scopedGreeter2.Greet()}",
                $"Singleton: {singletonGreeter1.Greet()}",
                $"Singleton: {singletonGreeter2.Greet()}"
            });
            return new OkObjectResult(result);
        }

        [FunctionName("LoggingGreeter")]
        public static IActionResult RunLoggingGreeter(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req,
            [Inject]LoggingGreeter greeter)
        {
            greeter.Greet();
            return new OkResult();
        }
    }
}
