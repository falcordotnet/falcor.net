using Sprache;

namespace Falcor.Server
{
    public class RouteBuilder<TRequest>
    {
        private readonly FalcorRouter<TRequest> _router;
        private readonly FalcorMethod _method;

        public RouteBuilder(FalcorMethod method, FalcorRouter<TRequest> router)
        {
            _method = method;
            _router = router;
        }

        public Route<TRequest> this[string route]
        {
            set { AddRoute(value, route); }
        }

        private void AddRoute(Route<TRequest> route, string routePath)
        {
            //var pathMatchers = RoutingGrammar.Route.Parse(routePath);
            //_router.Routes.Add(Route.From(_method, handler, pathMatchers));
        }
    }
}