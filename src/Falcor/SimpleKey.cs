namespace Falcor
{
    public abstract class SimpleKey : KeySegment
    {
        public override bool IsSimpleKey => true;
        public override KeySet AsKeySet() => new KeySet(this);

        public static implicit operator SimpleKey(string value) => new StringKey(value);

        public static implicit operator SimpleKey(bool value) => new BooleanKey(value);
    }
}