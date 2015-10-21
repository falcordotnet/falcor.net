using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public sealed class CompleteHandlerResult : RouteHandlerResult
    {
        private readonly List<PathValue> _values;

        public CompleteHandlerResult(List<PathValue> values)
        {
            _values = values;
        }

        public override RouteResult ToRouteResult(FalcorPath unmatchedPath) =>
            new CompleteRouteResult(unmatchedPath, _values);
    }
}