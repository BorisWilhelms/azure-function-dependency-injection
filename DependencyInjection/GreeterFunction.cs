using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace DependencyInjection
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [Inject]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }
    }
}
