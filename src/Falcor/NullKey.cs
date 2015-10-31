using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class NullKey : SimpleKey
    {
        public static NullKey Instance { get; } = new NullKey();
        public override KeyType KeyType { get; } = KeyType.Null;

        private bool Equals(NullKey other)
        {
            return KeyType == other.KeyType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is NullKey && Equals((NullKey) obj);
        }

        public override int GetHashCode()
        {
            return (int) KeyType;
        }

        public override JToken ToJToken() => JToken.FromObject(null);
    }
}