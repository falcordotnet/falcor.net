using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public abstract class KeySegment : IKeySegment, IJson
    {
        public abstract KeyType KeyType { get; }

        public bool IsString => KeyType == KeyType.String;
        public bool IsBoolean => KeyType == KeyType.Boolean;
        public bool IsNull => KeyType == KeyType.Null;
        public bool IsNumber => KeyType == KeyType.Number;
        public bool IsRange => KeyType == KeyType.Range;
        public bool IsRangeSet => KeyType == KeyType.RangeSet;
        public bool IsNumeric => IsNumber || IsRange;
        public bool IsNumericSet => IsNumeric || IsRangeSet;
        public bool IsKeySet => KeyType == KeyType.KeySet;
        public virtual bool IsSimpleKey => false;

        public abstract JToken ToJson();
        public virtual bool AsBoolean() => false;
        public virtual long AsInt() => 0;
        public virtual NumberRange AsRange() => new NumberRange(0, 0);
        public virtual NumericSet AsNumericSet() => new NumericSet();
        public virtual SortedSet<int> AsSortedNumberSet() => new SortedSet<int>();
        public virtual KeySet AsKeySet() => new KeySet();

        public static implicit operator KeySegment(string value) => new StringKey(value);
        public static implicit operator KeySegment(int value) => new NumberKey(value);
    }
}