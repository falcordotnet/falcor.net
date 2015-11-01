namespace Falcor.Server.Routing
{
    internal sealed class MatchingResult : MatchResult
    {
        public MatchingResult(string name = null, object value = null)
        {
            Name = name;
            Value = value;
        }


        public override bool IsMatched => true;
        public override bool HasName => Name != null;
        public override bool HasValue => Value != null;
        public override object Value { get; }
        public override string Name { get; }
    }
}