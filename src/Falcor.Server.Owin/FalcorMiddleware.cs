using System.Collections.Generic;
using System.Threading.Tasks;
using Falcor.Server.Routing;
using Microsoft.Owin;

namespace Falcor.Server.Owin
{
    public class FalcorMiddleware : OwinMiddleware
    {
        private readonly FalcorConfiguration _configuration;

        public FalcorMiddleware(OwinMiddleware next, FalcorConfiguration configuration) : base(next)
        {
            _configuration = configuration;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var queryStringPaths = context.Request.Query["paths"];
            var queryStringMethod = context.Request.Query["method"];
            //var method = queryStringMethod == "get" ? FalcorMethod.Get : queryStringMethod == "set" ? FalcorMethod.Set : FalcorMethod.Call;
            var method = FalcorMethod.Get;
            var paths = new List<FalcorPath> { new FalcorPath("foo", "bar", "baz") }; // TODO: Parse
            var request = new FalcorRequest(method, paths);
            var response = await _configuration.Router.RouteAsync(request);
            var serializer = new ResponseSerializer();
            var jsonResponse = serializer.Serialize(response);
            context.Response.Headers.Set("content-type", "application/json");
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}