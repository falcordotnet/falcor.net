using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class Atom : FalcorValue
    {
        public Atom(object value, int? size, int? expires = null)
        {
            Value = value;
            Expires = expires;
            Size = size;
        }

        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public object Value { get; }
        public int? Size { get; }

        public override bool IsValue => true;
        public int? Expires { get; }

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        public override JToken ToJToken()
        {
            var result = new JObject();
            result["$type"] = "atom";
            result["$timestamp"] = Timestamp.Ticks;
            if (Expires.HasValue) result["$expires"] = Expires;
            if (Size.HasValue) result["$size"] = Size;
            var value = SerializationHelper.SerializeItem(Value);
            result["value"] = value;
            return result;
        }

        protected override ValueType ValueType { get; } = ValueType.Atom;
    }

    internal static class SerializationHelper
    {
        public static JToken SerializeItem(object value)
        {
            var falcorValueOrKey = value as IJToken;
            if (falcorValueOrKey != null) return ((IJToken)value).ToJToken();

            if (value is int) return new JValue((int)value);
            var stringValue = value as string;
            if (stringValue != null) return new JValue(stringValue);

            var dict = value as IDictionary<string, object>;

            if (dict != null)
            {
                var obj = new JObject();
                foreach (var item in dict)
                {
                    obj[item.Key] = SerializeItem(item.Value);
                }
                return obj;
            }

            var array = value as IEnumerable<object>;
            if (array != null) return new JArray(array.Select(SerializeItem));
            return new JObject();
        }
    }
}