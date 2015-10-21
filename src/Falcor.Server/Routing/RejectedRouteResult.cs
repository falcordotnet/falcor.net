using System;
using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    internal class RejectedRouteResult : RouteResult
    {
        public RejectedRouteResult(string error)
        {
            Error = error;
        }

        public override bool IsComplete { get; } = false;
        public override ICollection<PathValue> Values { get { throw new InvalidOperationException(); } }
        public override FalcorPath UnmatchedPath { get { throw new InvalidOperationException(); } }
        public override string Error { get; }
    }
}