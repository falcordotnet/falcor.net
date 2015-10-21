using System;

namespace Falcor
{
    public sealed class StringKey : SimpleKey, IEquatable<StringKey>, IEquatable<string>
    {
        public string Value { get; }
        public override KeyType KeyType { get; } = KeyType.String;

        public StringKey(string value)
        {
            Util.ThrowIfArgumentNull(value, nameof(value));
            Value = value;
        }

        protected StringKey(StringKey key)
            : this(key.Value)
        { }


        public override bool Equals(object obj)
        {
            if (obj is string) return Equals((string) obj);
            return Equals(obj as StringKey);
        }

        public bool Equals(StringKey other)
        {
            return string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(StringKey lhs, StringKey rhs) => Util.IfBothNullOrEquals(lhs, rhs);

        public static bool operator !=(StringKey lhs, StringKey rhs) => !(lhs == rhs);

        public static implicit operator string (StringKey stringKey)
        {
            return stringKey.Value;
        }
        public static implicit operator StringKey(string value)
        {
            return new StringKey(value);
        }

        public bool Equals(string other)
        {
            return string.Equals(Value, other);
        }

        public override string ToString() => Value;
    }
}