using System;

namespace Falcor.Server
{
    /*
    public class RangesPatternMatcher : PathMatcher, IMatch<NumberRange>, IRoutePattern
    {
        protected bool Equals(RangesPatternMatcher other) => string.Equals(Name, other.Name);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumberRange
                ? Equals((NumberRange)obj)
                : obj is RangesPatternMatcher && Equals((RangesPatternMatcher)obj);
        }


        public bool IsNamed => Name != null;
        public string Name { get; }

        public RangesPatternMatcher() { }

        public RangesPatternMatcher(string name)
        {
            Name = name;
        }


        public bool Equals(NumberRange other) => true;


        public override int GetHashCode()
        {
            unchecked
            {
                return (397) ^ (Name?.GetHashCode() ?? 0);
            }
        }
    }
    */
}