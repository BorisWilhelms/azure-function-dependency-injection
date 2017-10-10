using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace DependencyInjection
{
    public static class GreeterFunction
    {
        [FunctionName("Greeter")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequestMessage req,
            [Inject(typeof(IGreeter))]IGreeter greeter)
        {
            return req.CreateResponse(greeter.Greet());
        }
    }
}
