
using DependencyInjection;
using Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionAppV2
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequest req,
            [Inject]IGreeter greeter,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            return new OkObjectResult(greeter.Greet());
        }
    }
}
