using System.Collections.Generic;
using Falcor.Server.Routing.PathParser;

namespace Falcor.Server.Routing
{
    public static class RouteCollectionExtensions
    {
        private static readonly IRouteParser RouteParser = FalcorRouterConfiguration.RouteParser;

        public static void Add(this RouteCollection routes, FalcorMethod method,
            IReadOnlyList<PathMatcher> pathMatchers, RouteHandler handler)
        {
            var route = handler
                .ToRoute()
                .MatchAndBindParameters(pathMatchers)
                .ForMethod(method);

            routes.Add(route);
        }

        public static void Add(this RouteCollection routes, FalcorMethod method, string path, RouteHandler handler) =>
            Add(routes, method, RouteParser.Parse(path), handler);
    }
}