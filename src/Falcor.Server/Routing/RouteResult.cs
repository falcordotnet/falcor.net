using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public abstract class RouteResult
    {
        public abstract bool IsComplete { get; }
        public abstract ICollection<PathValue> Values { get; }
        public abstract FalcorPath UnmatchedPath { get; }
        public abstract string Error { get; }

        public static RouteResult Reject() => new RejectedRouteResult(null);
        public static RouteResult Reject(string error) => new RejectedRouteResult(error);

        public static RouteResult Complete(FalcorPath unmatched, params PathValue[] values)
            => new CompleteRouteResult(unmatched, values);

        public static RouteResult Complete(FalcorPath unmatched, IEnumerable<PathValue> values)
            => new CompleteRouteResult(unmatched, values);
    }
}