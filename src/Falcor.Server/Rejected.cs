using System;
using System.Collections.Generic;

namespace Falcor.Server
{
    public class Rejected : RouteResult
    {
        public Rejected(string error)
        {
            Error = error;
        }

        public override bool IsComplete { get; } = false;

        public override ICollection<PathValue> Values
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override FalcorPath UnmatchedPath
        {
            get
            {

                throw new InvalidOperationException();
            }
        }
        public override string Error { get; }
    }
}