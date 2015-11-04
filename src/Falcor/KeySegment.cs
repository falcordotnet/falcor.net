using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public abstract class KeySegment : IKeySegment, IJson
    {
        public abstract KeyType KeyType { get; }
        public abstract JToken ToJson();
        public bool IsString() => KeyType == KeyType.String;
        public bool IsBoolean() => KeyType == KeyType.Boolean;
        public bool IsNull() => KeyType == KeyType.Null;
        public bool IsNumber() => KeyType == KeyType.Number;
        public bool IsRange() => KeyType == KeyType.Range;
        public bool IsRangeSet() => KeyType == KeyType.RangeSet;
        public bool IsNumeric() => IsNumber() || IsRange();
        public bool IsNumericSet() => IsNumeric() || IsRangeSet();
        public bool IsKeySet() => KeyType == KeyType.KeySet;
        public virtual bool AsBoolean() => false;
        public virtual long AsInt() => 0;
        public virtual NumberRange AsRange() => new NumberRange(0, 0);
        public virtual NumericSet AsNumericSet() => new NumericSet();
        public virtual SortedSet<int> AsSortedNumberSet() => new SortedSet<int>();
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

        public static implicit operator KeySegment(int value)
        {
            return new NumberKey(value);
        }
    }
}