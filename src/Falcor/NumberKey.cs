using System;

namespace Falcor
{
    public class NumberKey : NumericKey, IEquatable<NumberKey>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NumberKey && Equals((NumberKey) obj);
        }

        private readonly long _value;

        public NumberKey(long value)
        {
            _value = value;
        }

        public override long AsLong() => _value;

        public override NumberRange AsRange() => new NumberRange(_value);

        public override NumericSet AsNumericSet() => new NumericSet(_value);

        public override KeyType KeyType { get; } = KeyType.Number;

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public bool Equals(NumberKey other)
        {
            return other._value == _value;
        }
    }
}