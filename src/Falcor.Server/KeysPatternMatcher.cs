using System.Linq;

namespace Falcor.Server
{
    public class KeysPatternMatcher : PathMatcher, IMatch<KeySet>, IRoutePattern
    {
        protected bool Equals(KeysPatternMatcher other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (397) ^ (Name?.GetHashCode() ?? 0);
            }
        }

        public bool Equals(KeySet other) => true;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeySet ? Equals((KeySet)obj) :
                Equals((KeysPatternMatcher)obj);
        }

        public KeysPatternMatcher()
        {
            
        }

        public KeysPatternMatcher(string name)
        {
            Name = name;
        }


        public bool IsNamed => Name != null;
        public string Name { get; }
    }
}