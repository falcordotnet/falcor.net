namespace Falcor.Server.Routing
{
    public class ErrorHandlerResult : RouteHandlerResult
    {
        public ErrorHandlerResult(string error = null)
        {
            Error = error;
        }

        public string Error { get; }

        public override RouteResult ToRouteResult(FalcorPath unmatchedPath) => new RejectedRouteResult(Error);
    }
}