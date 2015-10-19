namespace Falcor.Server.Routing
{
    public abstract class MatchResult
    {
        public abstract bool IsMatched { get; }
        public abstract bool HasName { get; }
        public abstract bool HasValue { get; }
        public abstract object Value { get; }
        public abstract string Name { get; }
    }
}