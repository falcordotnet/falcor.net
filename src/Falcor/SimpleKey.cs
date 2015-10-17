using System.Collections.Generic;

namespace Falcor
{
    public abstract class SimpleKey : KeySegment
    {
        public override KeySet AsKeySet() => new KeySet(this);

        public static implicit operator SimpleKey(string value)
        {
            return new StringKey(value);
        }
    }
}