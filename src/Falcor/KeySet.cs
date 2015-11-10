using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class KeySet : KeySegment, IEnumerable<SimpleKey>
    {
        private readonly HashSet<SimpleKey> _keys;


        public KeySet(params SimpleKey[] keys) : this(keys.ToList())
        {
        }

        public KeySet(IEnumerable<SimpleKey> keys)
        {
            _keys = new HashSet<SimpleKey>(keys);
        }

        public KeySet(KeySet keySet)
            : this(keySet._keys)
        {
        }

        public override KeyType KeyType { get; } = KeyType.KeySet;

        [DebuggerStepThrough]
        public IEnumerator<SimpleKey> GetEnumerator() => _keys.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeySet && Equals((KeySet)obj);
        }

        public override KeySet AsKeySet() => this;
        public override JToken ToJson() => new JArray(_keys.Select(k => k.ToJson()));

        private bool Equals(KeySet other) => other != null && other._keys.SequenceEqual(_keys);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_keys?.GetHashCode() ?? 0) * 397) ^ (int)KeyType;
            }
        }
    }
}