using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public abstract class FalcorRouter : IRouter
    {
        public FalcorRouter()
        {
            Get = new RouteBuilder(FalcorMethod.Get, this);
            Set = new RouteBuilder(FalcorMethod.Set, this);
            Call = new RouteBuilder(FalcorMethod.Call, this);
        }

        private RoutingEngine RoutingEngine { get; } = new RoutingEngine();

        protected RouteBuilder Get { get; }
        protected RouteBuilder Set { get; }
        protected RouteBuilder Call { get; }

        public RouteCollection Routes => RoutingEngine.Routes;
        public Task<FalcorResponse> RouteAsync(FalcorRequest request) => RoutingEngine.RouteAsync(request);

        // Helpers
        public static IPathValueBuilder Path(params KeySegment[] keys)
            => new PathValueResultHelper(FalcorPath.From(keys));

        public static IPathValueBuilder Path(FalcorPath path) => new PathValueResultHelper(path);
        public static RouteHandlerResult Complete(PathValue value) => Complete(new List<PathValue> {value});

        public static RouteHandlerResult Complete(IEnumerable<IEnumerable<PathValue>> values)
            => Complete(values.SelectMany(v => v.ToList()));

        public static RouteHandlerResult Complete(IEnumerable<PathValue> values)
            => new CompleteHandlerResult(values.ToList());

        public static RouteHandlerResult Error(string error = null) => new ErrorHandlerResult(error);

        protected class RouteBuilder
        {
            private readonly FalcorMethod _method;
            private readonly IRouter _router;

            public RouteBuilder(FalcorMethod method, IRouter router)
            {
                _method = method;
                _router = router;
            }

            public RouteHandler this[string path]
            {
                set { _router.Routes.Add(_method, path, value); }
            }
        }
    }
}