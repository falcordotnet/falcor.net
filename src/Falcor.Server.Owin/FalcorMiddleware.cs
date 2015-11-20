using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server.Routing;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace Falcor.Server.Owin
{
    public sealed class FalcorMiddleware : OwinMiddleware
    {
        public static FalcorRouterConfiguration RouterConfiguration { get; private set; }

        public FalcorMiddleware(OwinMiddleware next, FalcorRouterConfiguration routerConfiguration) : base(next)
        {
            RouterConfiguration = routerConfiguration;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var bodyStream = context.Request.Body;
            var sr = new StreamReader(bodyStream);
            var body = sr.ReadToEnd();

            string queryStringPaths = null;
            string queryStringMethod = null;
            dynamic jsonGraph = null;
            if (string.IsNullOrEmpty(body))
            {
                queryStringPaths = context.Request.Query["paths"];
                queryStringMethod = context.Request.Query["method"];
            }
            else
            {
                var parts = body.Split('&');
                var jsonGraphEnvelopeString = parts[0].Split('=')[1];
                dynamic jsonGraphEnvelope = JsonConvert.DeserializeObject(jsonGraphEnvelopeString);
                queryStringPaths = JsonConvert.SerializeObject(jsonGraphEnvelope.paths);
                queryStringMethod = parts[1].Split('=')[1];
                jsonGraph = jsonGraphEnvelope.jsonGraph;
            }

            var method = queryStringMethod == "get" ? FalcorMethod.Get : queryStringMethod == "set" ? FalcorMethod.Set : FalcorMethod.Call;
            var paths = FalcorRouterConfiguration.PathParser.ParseMany(queryStringPaths);
            var request = new FalcorRequest(method, paths,jsonGraph);
            var response = await RouterConfiguration.Router.RouteAsync(request);
            var jsonResponse = FalcorRouterConfiguration.ResponseSerializer.Serialize(response);
            context.Response.Headers.Set("content-type", "application/json");
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}