namespace Falcor
{
    public sealed class BooleanKey : SimpleKey
    {
        private readonly bool _value;

        public BooleanKey(bool value)
        {
            _value = value;
        }

        public override KeyType KeyType { get; } = KeyType.Boolean;
        public override bool AsBoolean() => _value;

        private bool Equals(BooleanKey other)
        {
            return _value == other._value && KeyType == other.KeyType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is BooleanKey && Equals((BooleanKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_value.GetHashCode() * 397) ^ (int)KeyType;
            }
        }

    }
}