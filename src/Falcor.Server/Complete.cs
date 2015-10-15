using System;
using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server
{
    public sealed class Complete : RouteResult
    {
        public Complete(FalcorPath unmatchedPath, IEnumerable<PathValue> values)
        {
            UnmatchedPath = unmatchedPath;
            Values = values.ToArray();
        }

        public override bool IsComplete { get; } = true;
        public override ICollection<PathValue> Values { get; }
        public override FalcorPath UnmatchedPath { get; }

        public override string Error
        {
            get
            {
                throw new InvalidOperationException();
            }
        }
    }
}