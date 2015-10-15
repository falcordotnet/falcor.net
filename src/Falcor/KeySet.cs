using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Falcor
{
    public sealed class KeySet : KeySegment, IEnumerable<SimpleKey>
    {
        public IEnumerator<SimpleKey> GetEnumerator() => Keys.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is KeySet && Equals((KeySet)obj);
        }

        protected readonly HashSet<SimpleKey> Keys;
        public override KeyType KeyType { get; } = KeyType.KeySet;


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

        public override HashSet<SimpleKey> AsKeySet() => Keys;

        private bool Equals(KeySet other) => other != null && other.Keys.SequenceEqual(Keys);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Keys?.GetHashCode() ?? 0) * 397) ^ (int)KeyType;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}