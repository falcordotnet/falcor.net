using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Falcor.Server
{
    public abstract class RouteResult
    {
        public abstract bool IsComplete { get; }
        public abstract ICollection<PathValue> Values { get; }
        public abstract FalcorPath UnmatchedPath { get; }
        public abstract string Error { get; }

        public static RouteResult Reject() => new Rejected(null);
        public static RouteResult Reject(string error) => new Rejected(error);
        public static RouteResult Complete(FalcorPath unmatched, params PathValue[] values) => new Complete(unmatched, values);
        public static RouteResult Complete(FalcorPath unmatched, IEnumerable<PathValue> values) => new Complete(unmatched, values);
    }
}