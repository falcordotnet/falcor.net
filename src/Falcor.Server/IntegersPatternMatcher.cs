using System;

namespace Falcor.Server
{
    public class IntegersPatternMatcher : PathMatcher, IMatch<NumericSet>, IMatch<NumberKey>, IRoutePattern
    {
        public IntegersPatternMatcher()
        {

        }

        public IntegersPatternMatcher(string name)
        {
            Name = name;
        }

        public bool Equals(IntegersPatternMatcher other) => other.Name == Name;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumericSet ? Equals((NumericSet)obj)
                : obj is NumberKey ? Equals((NumberKey)obj)
                    : obj is IntegersPatternMatcher && Equals((IntegersPatternMatcher)obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public bool Equals(NumericSet other) => true;

        public bool Equals(NumberKey other) => true;
        public bool IsNamed => Name != null;
        public string Name { get; }
    }
}