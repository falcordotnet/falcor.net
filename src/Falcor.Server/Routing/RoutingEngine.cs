using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public class RoutingEngine : IRouter
    {
        private readonly FalcorResponseBuilder _responseBuilder = new FalcorResponseBuilder();

        private Lazy<Route> LazyRootRoute => new Lazy<Route>(() => Routes.FirstToComplete());
        private Route RootRoute => LazyRootRoute.Value;
        public RouteCollection Routes { get; } = new RouteCollection();

        public async Task<FalcorResponse> RouteAsync(FalcorRequest request)
        {
            var result = await Route(request).ToList().ToTask();
            var response = _responseBuilder.CreateResponse();
            return response;
        }

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
                                    var unmatched = ((Ref) pathValue.Value).AsRef().AppendAll(result.UnmatchedPath);
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
            request.Paths.ToObservable()
                .SelectMany(unmatched => Resolve(RootRoute, new RequestContext(request, unmatched)));
    }
}