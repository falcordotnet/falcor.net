using System;

namespace Falcor.Server.Routing
{
    public class UnmatchedResult : MatchResult
    {
        public override bool HasValue { get; } = false;
        public override object Value { get; } = null;
        public override bool IsMatched => false;
        public override bool HasName => false;
        public override string Name { get { throw new InvalidOperationException(); } }
    }
}