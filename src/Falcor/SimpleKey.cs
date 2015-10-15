using System.Collections.Generic;

namespace Falcor
{
    public abstract class SimpleKey : KeySegment
    {
        public override HashSet<SimpleKey> AsKeySet() => new HashSet<SimpleKey> { this };

        public static implicit operator SimpleKey(string value)
        {
            return new StringKey(value);
        }
    }
}