namespace Falcor.Server
{
    public static class RouteExtensions
    {
        public static Route<TRequest> WithRequest<TRequest>(this Route<TRequest> route, FalcorRequest<TRequest> request) =>
            context => route(context.WithRequest(request));

        public static Route<TRequest> WithPaths<TRequest>(this Route<TRequest> route, FalcorPath matched, FalcorPath unmatched) =>
            context => route(context.WithPaths(matched, unmatched));
    }
}