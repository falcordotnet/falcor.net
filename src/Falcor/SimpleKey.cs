namespace Falcor
{
    public abstract class SimpleKey : KeySegment
    {
        public override KeySet AsKeySet() => new KeySet(this);

        public static implicit operator SimpleKey(string value)
        {
            return new StringKey(value);
        }

        public static implicit operator SimpleKey(bool value)
        {
            return new BooleanKey(value);
        }
    }
}