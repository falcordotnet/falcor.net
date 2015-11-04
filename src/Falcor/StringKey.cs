using System;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class StringKey : SimpleKey, IEquatable<StringKey>, IEquatable<string>
    {
        public StringKey(string value)
        {
            Util.ThrowIfArgumentNull(value, nameof(value));
            Value = value;
        }

        public string Value { get; }
        public override KeyType KeyType { get; } = KeyType.String;


        public bool Equals(string other) => string.Equals(Value, other);

        public bool Equals(StringKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Value, other.Value) && KeyType == other.KeyType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string) return Equals((string) obj);
            return obj is StringKey && Equals((StringKey) obj);
        }

        public override JToken ToJson() => JToken.FromObject(Value);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Value != null ? Value.GetHashCode() : 0)*397) ^ (int) KeyType;
            }
        }

        public static implicit operator string(StringKey stringKey) => stringKey.Value;
        public static implicit operator StringKey(string value) => new StringKey(value);
        public override string ToString() => Value;
    }
}