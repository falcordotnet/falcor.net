namespace Falcor.Server.Routing
{
    public class RouteBuilder
    {
        private readonly FalcorRouter _router;
        private readonly FalcorMethod _method;
        private static readonly IRouteParser RouteParser = new MemoizedRouteParser(new SpracheRouteParser());

        public RouteBuilder(FalcorMethod method, FalcorRouter router)
        {
            _method = method;
            _router = router;
        }

        public RouteHandler this[string path] { set { _router.Routes.Add(BuildRoute(value, path)); } }

        public Route BuildRoute(RouteHandler handler, string path) =>
            handler.ToRoute()
            .MatchAndBindParameters(RouteParser.Parse(path))
            .ForMethod(_method);
    }
}