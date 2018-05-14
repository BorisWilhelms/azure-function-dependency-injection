using DependencyInjection;
using Lib;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionAppV1
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")]HttpRequestMessage req,
            [Inject]IGreeter greeter,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            return req.CreateResponse(HttpStatusCode.OK, greeter.Greet());
        }
    }
}
