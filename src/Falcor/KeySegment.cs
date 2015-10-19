using System.Collections.Generic;

namespace Falcor
{
    public interface IKeySegment { }
    public abstract class KeySegment : IKeySegment
    {
        public abstract KeyType KeyType { get; }
        public bool IsString() => KeyType == KeyType.String;
        public bool IsBoolean() => KeyType == KeyType.Boolean;
        public bool IsNull() => KeyType == KeyType.Null;
        public bool IsNumber() => KeyType == KeyType.Number;
        public bool IsRange() => KeyType == KeyType.Range;
        public bool IsRangeSet() => KeyType == KeyType.RangeSet;
        public bool IsNumeric() => IsNumber() || IsRange();
        public bool IsNumericSet() => IsNumeric() || IsRangeSet();
        public bool IsKeySet() => KeyType == KeyType.KeySet;
        public bool AsBoolean() => false;
        public virtual long AsLong() => 0;
        public virtual NumberRange AsRange() => new NumberRange(0, 0);
        public virtual NumericSet AsNumericSet() => new NumericSet();
        public virtual SortedSet<long> AsSortedNumberSet() => new SortedSet<long>();
        public virtual KeySet AsKeySet() => new KeySet();

        //        protected abstract string StringValue { get; set; }
        //public static implicit operator string (KeySegment keySegment)
        //{
        //    var sk = keySegment as StringKey;
        //    if (sk != null)
        //    {
        //        return sk.StringValue;
        //    }
        //    return null;
        //}
        public static implicit operator KeySegment(string value)
        {
            return new StringKey(value);
        }

        public static implicit operator KeySegment(long value)
        {
            return new NumberKey(value);
        }
    }
}