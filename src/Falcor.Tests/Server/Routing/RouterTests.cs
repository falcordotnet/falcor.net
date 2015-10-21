using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class RouterTests
    {
        //private static FalcorRouterConfiguration _routerConfiguration = FalcorRouterConfiguration.Create("model.json", config => new TestRouter());

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
}
