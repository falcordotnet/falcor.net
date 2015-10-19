namespace Falcor.Server.Routing
{
    public abstract class RouteHandlerResult
    {
        public abstract RouteResult ToRouteResult(FalcorPath unmatchedPath);
    }
}