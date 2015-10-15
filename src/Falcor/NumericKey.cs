namespace Falcor
{
    public abstract class NumericKey : KeySegment
    {

        public static implicit operator NumericKey(long value)
        {
            return new NumberKey(value);
        }
    }
}