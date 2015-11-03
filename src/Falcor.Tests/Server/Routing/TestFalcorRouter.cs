using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server;
using Falcor.Server.Routing;

namespace Falcor.Tests.Server.Routing
{
    public class TestFalcorRouter : FalcorRouter
    {
        public TestFalcorRouter()
        {
            Get["foo[{integers:ids}].name"] = parameters =>
            {
                NumericSet ids = parameters.ids;
                var results = ids.Select(id => new PathValue(FalcorPath.From("foo", id, "name"), "Jill-" + id));
                return Complete(results);
            };

            Get["foo"] = parameters => Complete(new PathValue(new FalcorPath("foo"), "bar"));

			Get["foo[{keys:ids}].name"] = parameters => {
				KeySet ids = parameters.ids;
				var results = ids.Select(id => new PathValue(FalcorPath.From("foo", id, "name"), "Jill-" + id));
				return Complete(results);
			};
		}

        // Test helper methods
        public static Task<RouteHandlerResult> Complete(params PathValue[] values) => Complete(values.ToList());

        public static Task<RouteHandlerResult> Complete(IEnumerable<PathValue> values)
            => Task.FromResult(FalcorRouter.Complete(values.ToList()));
    }
}