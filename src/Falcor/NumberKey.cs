using System;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public class NumberKey : NumericKey, IEquatable<NumberKey>
    {
        private readonly int _value;

        public NumberKey(int value)
        {
            _value = value;
        }

        public override KeyType KeyType { get; } = KeyType.Number;

        public bool Equals(NumberKey other)
        {
            return other._value == _value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumberKey && Equals((NumberKey) obj);
        }

        public override long AsInt() => _value;

        public override NumberRange AsRange() => new NumberRange(_value);

        public override NumericSet AsNumericSet() => new NumericSet(_value);
        public override JToken ToJToken() => JToken.FromObject(_value);

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString() => _value.ToString();
    }
}