using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class KeySet : KeySegment, IEnumerable<SimpleKey>
    {
        protected readonly HashSet<SimpleKey> Keys;


        public KeySet(params SimpleKey[] keys) : this(keys.ToList())
        {
        }

        public KeySet(IEnumerable<SimpleKey> keys)
        {
            Keys = new HashSet<SimpleKey>(keys);
        }

        public KeySet(KeySet keySet)
            : this(keySet.Keys)
        {
        }

        public override KeyType KeyType { get; } = KeyType.KeySet;

        [DebuggerStepThrough]
        public IEnumerator<SimpleKey> GetEnumerator() => Keys.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeySet && Equals((KeySet) obj);
        }

        public override KeySet AsKeySet() => this;
        public override JToken ToJson() => new JArray(Keys.Select(k => k.ToJson()));

        private bool Equals(KeySet other) => other != null && other.Keys.SequenceEqual(Keys);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Keys?.GetHashCode() ?? 0)*397) ^ (int) KeyType;
            }
        }
    }
}