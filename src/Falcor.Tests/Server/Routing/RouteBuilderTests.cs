using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class RouteBuilderTests
    {
        [Scenario]
        public void CanBuildRoute()
        {
            var routeBuilder = new RouteBuilder(FalcorMethod.Get, null);
            var route = routeBuilder.BuildRoute(parameters => Task.FromResult(FalcorRouter.Complete(new PathValue(FalcorPath.From("foo"), "bar"))), "foo");
            var result = route(new RequestContext(new FalcorRequest(FalcorMethod.Get, new List<FalcorPath>()), FalcorPath.From("foo"))).ToTask().Result;
            Assert.Equal("bar", result.Values.First().Value);
        }
    }
}