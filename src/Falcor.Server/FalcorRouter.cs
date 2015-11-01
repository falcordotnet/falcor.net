using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public abstract class FalcorRouter : IFalcorRouter
    {
        protected FalcorRouter()
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
        protected static IPathValueBuilder Path(params KeySegment[] keys)
            => new PathValueResultHelper(FalcorPath.From(keys));

        public static IPathValueBuilder Path(FalcorPath path) => new PathValueResultHelper(path);
        public static RouteHandlerResult Complete(PathValue value) => Complete(new List<PathValue> {value});

        protected static RouteHandlerResult Complete(IEnumerable<IEnumerable<PathValue>> values)
            => Complete(values.SelectMany(v => v.ToList()));

        protected static RouteHandlerResult Complete(IEnumerable<PathValue> values)
            => new CompleteHandlerResult(values.ToList());

        protected static RouteHandlerResult Error(string error = null) => new ErrorHandlerResult(error);

        protected class RouteBuilder
        {
            private readonly IFalcorRouter _falcorRouter;
            private readonly FalcorMethod _method;

            public RouteBuilder(FalcorMethod method, IFalcorRouter falcorRouter)
            {
                _method = method;
                _falcorRouter = falcorRouter;
            }

            public RouteHandler this[string path]
            {
                set { _falcorRouter.Routes.Add(_method, path, value); }
            }
        }
    }
}