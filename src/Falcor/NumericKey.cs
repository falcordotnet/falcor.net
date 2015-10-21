namespace Falcor
{
    public abstract class NumericKey : KeySegment
    {

        public static implicit operator NumericKey(int value)
        {
            return new NumberKey(value);
        }
    }
}