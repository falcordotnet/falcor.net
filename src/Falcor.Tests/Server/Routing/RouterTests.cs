using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Falcor.Server.Routing;
using Newtonsoft.Json;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class RouterTests
    {
        [Scenario]
        public void GetFoo()
        {
            var router = new TestFalcorRouter();
            var request = FalcorRequest.Get("foo");
            var response = router.RouteAsync(request).Result;
            Assert.Equal("bar", response.JsonGraph["foo"]);
        }

        [Scenario]
        public void GetWithIntegers()
        {
            var router = new TestFalcorRouter();
            var request = FalcorRequest.Get("foo", new NumericSet(1, 2, 3), "name");
            var response = router.RouteAsync(request).Result;
            var foos = (Dictionary<string, object>) response.JsonGraph["foo"];
            Assert.Equal(3, foos.Count);
            Assert.Equal(new List<string> {"1", "2", "3"}, foos.Select(kv => kv.Key));
            Assert.Equal(new List<string> {"Jill-1", "Jill-2", "Jill-3"},
                foos.Select(kv => ((Dictionary<string, object>) kv.Value)["name"]));
        }

        [Scenario]
        public void SetSingleValue()
        {
            var router = new TestFalcorRouter();
            var jsonGraphEnvelopeString = "{\"jsonGraph\":{\"todos\":{\"1\":{\"done\":false}}},\"paths\":[[\"todos\",1,\"done\"]]}";
            dynamic jsonGraphEnvelope = JsonConvert.DeserializeObject(jsonGraphEnvelopeString);
            var paths = FalcorRouterConfiguration.PathParser.ParseMany(JsonConvert.SerializeObject(jsonGraphEnvelope.paths));

            var falcorRequest = new FalcorRequest(FalcorMethod.Set, paths, jsonGraphEnvelope.jsonGraph);
            var falcorResponse = router.RouteAsync(falcorRequest).Result;

            Assert.Equal(((Dictionary<string, object>) falcorResponse.JsonGraph["todos"])
                .Any(kv1 => kv1.Key.Equals("1") && ((Dictionary<string, object>) kv1.Value)
                .Any(kv2 => kv2.Key.Equals("done") && ((Atom)kv2.Value).Value.Equals(false))), true);
        }
    }
}