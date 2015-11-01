using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Falcor.Server.Routing
{
    public class RoutingEngine : IFalcorRouter
    {
        private readonly List<PathValue> _results = new List<PathValue>();
        private Lazy<Route> LazyRootRoute => new Lazy<Route>(() => Routes.FirstToComplete());
        private Route RootRoute => LazyRootRoute.Value;
        public RouteCollection Routes { get; } = new RouteCollection();

        public async Task<FalcorResponse> RouteAsync(FalcorRequest request)
        {
            await Route(request).ToList().ToTask();
            var response = FalcorResponse.From(_results);
            return response;
        }

        private IObservable<PathValue> Resolve(Route route, RequestContext context)
        {
            if (!context.Unmatched.Any() || _results.Any(pv => pv.Path.Equals(context.Unmatched)))
                return Observable.Empty<PathValue>();

            var results = route(context).SelectMany(result =>
            {
                if (result.IsComplete)
                {
                    var pathValues = result.Values;
                    if (pathValues.Any())
                    {
                        _results.AddRange(pathValues);
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
                    _results.Add(new PathValue(context.Unmatched, error));
                    return Observable.Return(new PathValue(context.Unmatched, error));
                }
                return Observable.Empty<PathValue>();
            });

            return results;
        }

        private IObservable<PathValue> Route(FalcorRequest request) =>
            request.Paths.ToObservable()
                .SelectMany(unmatched => Resolve(RootRoute, new RequestContext(request, unmatched)));
    }
}