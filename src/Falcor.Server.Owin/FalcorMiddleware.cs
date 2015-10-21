using System.Collections.Generic;
using System.Threading.Tasks;
using Falcor.Server.Routing;
using Microsoft.Owin;

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
            var queryStringPaths = context.Request.Query["paths"];
            var queryStringMethod = context.Request.Query["method"];
            //var method = queryStringMethod == "get" ? FalcorMethod.Get : queryStringMethod == "set" ? FalcorMethod.Set : FalcorMethod.Call;
            var method = FalcorMethod.Get;
            var paths = FalcorRouterConfiguration.PathParser.ParseMany(queryStringPaths);
            var request = new FalcorRequest(method, paths);
            var response = await RouterConfiguration.Router.RouteAsync(request);
            var jsonResponse = FalcorRouterConfiguration.ResponseSerializer.Serialize(response);
            context.Response.Headers.Set("content-type", "application/json");
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}