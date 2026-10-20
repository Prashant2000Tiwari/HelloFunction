using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HelloFunction
{
    public class HelloWorldApi
    {
        private readonly ILogger _logger;

        public HelloWorldApi(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloWorldApi>();
        }

        [Function("HelloWorldApi")]
        public HttpResponseData Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "hello/{name?}"
            )] HttpRequestData req,
            string? name)
        {
            _logger.LogInformation("HelloWorldApi function executed.");

            // If name is not provided in route,
            // check query string ?name=Prashant
            if (string.IsNullOrEmpty(name))
            {
                var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
                name = query["name"];
            }

            string message;

            if (string.IsNullOrEmpty(name))
            {
                message = "Hello, World! API";
            }
            else
            {
                message = $"Hello, World! API {name}";
            }

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add(
                "Content-Type",
                "text/plain; charset=utf-8"
            );

            response.WriteString(message);

            return response;
        }
    }
}