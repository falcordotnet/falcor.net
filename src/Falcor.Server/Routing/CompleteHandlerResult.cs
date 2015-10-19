using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public class CompleteHandlerResult : RouteHandlerResult
    {
        public CompleteHandlerResult(List<PathValue> values)
        {
            Values = values;
        }

        public List<PathValue> Values { get; }

        public override RouteResult ToRouteResult(FalcorPath unmatchedPath) =>
            new CompleteRouteResult(unmatchedPath, Values);
    }
}