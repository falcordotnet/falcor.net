namespace Falcor.Server.Routing
{
    public class MatchingResult : MatchResult
    {
        public MatchingResult(string name = null, object value = null)
        {
            Name = name;
            Value = value;
        }



        public override bool IsMatched => true;
        public override bool HasName => Name != null;
        public override bool HasValue => Value != null;
        public override sealed object Value { get; }
        public override sealed string Name { get; }
    }
}