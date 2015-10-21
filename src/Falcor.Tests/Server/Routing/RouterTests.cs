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
    public class TestRouter : FalcorRouter
    {
        public TestRouter()
        {
            Get["foo[{integers:ids}].name"] = parameters =>
            {
                NumericSet ids = parameters.ids;
                var results = ids.Select(id => new PathValue(FalcorPath.From("foo", id, "name"), "Jill-" + id));
                return Complete(results);
            };

            Get["foo"] = parameters => Complete(new PathValue(new FalcorPath("foo"), "bar"));
        }

        // Test router helper methods
        public static Task<RouteHandlerResult> Complete(params PathValue[] values) => Complete(values.ToList());

        public static Task<RouteHandlerResult> Complete(IEnumerable<PathValue> values)
            => Task.FromResult(FalcorRouter.Complete(values.ToList()));
    }

    public class RouterTests
    {
        [Scenario]
        public void GetFoo()
        {
            var router = new TestRouter();
            var request = FalcorRequest.Get("foo");
            var response = router.RouteAsync(request).Result;
            Assert.Equal("bar", response.JsonGraph["foo"]);
        }

        [Scenario]
        public void GetWithIntegers()
        {
            var router = new TestRouter();
            var request = FalcorRequest.Get("foo", new NumericSet(1, 2, 3), "name");
            var response = router.RouteAsync(request).Result;
            var foos = (Dictionary<string, object>)response.JsonGraph["foo"];
            Assert.Equal(3, foos.Count);
            Assert.Equal(new List<string> { "1", "2", "3" }, foos.Select(kv => kv.Key));
            Assert.Equal(new List<string> { "Jill-1", "Jill-2", "Jill-3" }, foos.Select(kv => ((Dictionary<string, object>)kv.Value)["name"]));
        }

    }

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
