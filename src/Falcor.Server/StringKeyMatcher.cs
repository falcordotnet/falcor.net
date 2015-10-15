using System;

namespace Falcor.Server
{
    public class StringKeyMatcher : PathMatcher, IMatch<StringKey>
    {
        protected bool Equals(StringKeyMatcher other) => string.Equals(_value, other._value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is StringKey ?
                Equals((StringKey)obj) :
                Equals((StringKeyMatcher)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (397) ^ (_value?.GetHashCode() ?? 0);
            }
        }

        private readonly string _value;

        public StringKeyMatcher(string value)
        {
            _value = value;
        }

        public bool Equals(StringKey other) => other.Value == _value;

    }
}
