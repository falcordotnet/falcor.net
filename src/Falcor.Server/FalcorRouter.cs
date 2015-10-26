using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Falcor.Server.Routing;

namespace Falcor.Server
{
    public abstract class FalcorRouter
    {
        public List<Route> Routes { get; } = new List<Route>();
        private readonly FalcorResponseBuilder _responseBuilder = new FalcorResponseBuilder();
        private Lazy<Route> LazyRootRoute => new Lazy<Route>(() => Routes.FirstToComplete());
        private Route RootRoute => LazyRootRoute.Value;

        protected RouteBuilder Get => new RouteBuilder(FalcorMethod.Get, this);
        protected RouteBuilder Set => new RouteBuilder(FalcorMethod.Set, this);
        protected RouteBuilder Call => new RouteBuilder(FalcorMethod.Call, this);

        // Helpers
        public static IPathValueBuilder Path(params KeySegment[] keys) => new PathValueBuilder(FalcorPath.From(keys));
        public static IPathValueBuilder Path(FalcorPath path) => new PathValueBuilder(path);
        public static RouteHandlerResult Complete(PathValue value) => Complete(new List<PathValue> { value });
        public static RouteHandlerResult Complete(IEnumerable<IEnumerable<PathValue>> values) => Complete(values.SelectMany(v => v.ToList()));
        public static RouteHandlerResult Complete(IEnumerable<PathValue> values) => new CompleteHandlerResult(values.ToList());
        public static RouteHandlerResult Error(string error = null) => new ErrorHandlerResult(error);

        // Routing
        private IObservable<PathValue> Resolve(Route route, RequestContext context)
        {
            if (!context.Unmatched.Any() || _responseBuilder.Contains(context.Unmatched))
                return Observable.Empty<PathValue>();

            var results = route(context).SelectMany(result =>
            {
                if (result.IsComplete)
                {
                    var pathValues = result.Values;
                    if (pathValues.Any())
                    {
                        _responseBuilder.AddRange(pathValues);
                        if (result.UnmatchedPath.Any())
                        {
                            return pathValues.ToObservable()
                                .Where(pathValue => pathValue.Value is Ref)
                                .SelectMany(pathValue =>
                                {
                                    var unmatched = ((Ref)pathValue.Value).AsRef().AppendAll(result.UnmatchedPath);
                                    return Resolve(route, context.WithUnmatched(unmatched));
                                })
                                //.StartWith(pathValues) // Is this nescessary?
                                ;
                        }
                    }
                }
                else
                {
                    var error = new Error(result.Error);
                    _responseBuilder.Add(context.Unmatched, error);
                    return Observable.Return(new PathValue(context.Unmatched, error));
                }
                return Observable.Empty<PathValue>();
            });

            return results;
        }

        public IObservable<PathValue> Route(FalcorRequest request) =>
            request.Paths.ToObservable().SelectMany(unmatched => Resolve(RootRoute, new RequestContext(request, unmatched)));

        public async Task<FalcorResponse> RouteAsync(FalcorRequest request)
        {
            IList<PathValue> result = await Route(request).ToList().ToTask();
            var response = _responseBuilder.CreateResponse();
            return response;
        }
    }
}