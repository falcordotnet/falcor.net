using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Falcor.Server.Routing.PathParser;

namespace Falcor.Server.Routing
{
    public static class RouteHelpers
    {
        public static Route FirstToComplete(this RouteCollection routes) =>
            context => FirstToComplete(routes, context, 0);

        private static IObservable<RouteResult> FirstToComplete(RouteCollection routes, RequestContext context,
            int index)
        {
            if (routes.Count > index)
            {
                return routes[index](context)
                    .Where(r => r.IsComplete)
                    .SwitchIfEmpty(Observable.Defer(() => FirstToComplete(routes, context, ++index)));
            }
            return context.Reject();
        }

        public static Route ForMethod(this Route inner, FalcorMethod routeMethod) =>
            ExtractMethod(requestMethod => routeMethod == requestMethod ? inner : Reject());

        public static Route Reject(string error = null) => context => context.Reject(error);

        private static Route ExtractMethod(Func<FalcorMethod, Route> inner)
            => Extract(context => context.Request.Method, inner);

        public static Route Extract<T>(Func<RequestContext, T> extractor, Func<T, Route> inner)
            => context => inner(extractor(context))(context);


        internal static Route MatchAndBindParameters(this Route inner, IReadOnlyList<PathMatcher> pathMatchers)
        {
            return context =>
            {
                if (pathMatchers.Count > context.Unmatched.Count) return context.Reject();

                var matches = new List<MatchResult>();

                // Use a for loop so we can return a rejection early if there is no match
                for (var i = 0; i < pathMatchers.Count; i++)
                {
                    var matchResult = pathMatchers[i](context.Unmatched[i]);
                    if (!matchResult.IsMatched) return context.Reject();
                    matches.Add(matchResult);
                }

                var unmatched = new FalcorPath(context.Unmatched.Skip(pathMatchers.Count));
                var parameters = new DynamicDictionary();
                matches.Where(m => m.HasValue && m.HasValue).ToList().ForEach(m => parameters.Add(m.Name, m.Value));
                parameters.Add("jsonGraph",context.Request.JsonGraph);
                return inner(context.WithUnmatched(unmatched, parameters))
                    // Only allow partial matches if we have a ref in the results
                    .Select(
                        result =>
                            result.IsComplete && result.UnmatchedPath.Any() && !result.Values.Any(pv => pv.Value is Ref)
                                ? RouteResult.Reject($"Unhandled key segments: {result.UnmatchedPath}")
                                : result);
            };
        }

        public static Route ToRoute(this RouteHandler handler)
        {
            return
                context =>
                {
                    return
                        ((Task<RouteHandlerResult>) handler(context.Parameters)).ToObservable()
                            .Select(handlerResult => handlerResult.ToRouteResult(context.Unmatched));
                };
        }
    }
}