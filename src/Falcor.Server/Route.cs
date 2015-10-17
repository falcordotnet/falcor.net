using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Falcor.Server
{
    public delegate IObservable<RouteResult> Route<TRequest>(RequestContext<TRequest> context);

    public static class Route
    {
        public static Route<TRequest> FirstOf<TRequest>(List<Route<TRequest>> routes) =>
            context => context.FirstOf(routes);

        public static Route<TRequest> From<TRequest>(List<PathMatcher> pathMatchers,
            Route<TRequest> inner)
        {
            return context =>
            {
                var possibleMatch = pathMatchers.Count <= context.Unmatched.Count;

                if (possibleMatch)
                {
                    for (var i = 0; i < pathMatchers.Count; i++)
                    {
                        var matchResult = pathMatchers[i](context.Unmatched[i]);
                        if (!matchResult.IsMatched)
                        {
                            possibleMatch = false;
                            break;
                        }
                    }
                }
                if (possibleMatch)
                {
                    var matched = new FalcorPath(context.Unmatched.Take(pathMatchers.Count));
                    var unmatched = new FalcorPath(context.Unmatched.Skip(pathMatchers.Count));
                    // TODO: Populate values 
                    return inner(context.WithPaths(matched, unmatched));
                }
                return Observable.Return(RouteResult.Reject());
            };
        }

        public static Route<TRequest> From<TRequest>(Func<RequestContext<TRequest>, Task<RouteResult>> asyncRoute) =>
            context => asyncRoute(context).ToObservable();

        public static Route<TRequest> WithRequest<TRequest>(this Route<TRequest> action, FalcorRequest<TRequest> request) =>
                context => action(context.WithRequest(request));

        public static Route<TRequest> WithPaths<TRequest>(this Route<TRequest> action, FalcorPath matched, FalcorPath unmatched) =>
            context => action(context.WithPaths(matched, unmatched));
    }


}
