using System;
using System.Linq;

namespace Falcor.Server
{
    public class KeySetMatcher : PathMatcher, IMatch<StringKey>, IMatch<KeySet>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeySetMatcher ? Equals((KeySetMatcher)obj)
                : obj is KeySet ? Equals((KeySet)obj)
                : obj is StringKey && Equals((StringKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (397) ^ (_keySet?.GetHashCode() ?? 0);
            }
        }

        private readonly KeySet _keySet;

        public KeySetMatcher(params string[] keys)
            : this(new KeySet(keys.Select(k => new StringKey(k))))
        {

            
        }

        public KeySetMatcher(KeySet keySet)
        {
            _keySet = keySet;
        }

        public bool Equals(StringKey other) => _keySet.Contains(other);

        protected bool Equals(KeySetMatcher other) => Equals(_keySet, other._keySet);

        public bool Equals(KeySet other) => Equals(_keySet, other);

        public static implicit operator KeySetMatcher(KeySet keySet) => new KeySetMatcher(keySet);
    }
}