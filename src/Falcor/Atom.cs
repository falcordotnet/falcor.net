using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    public sealed class Atom : FalcorValue
    {
        public Atom(object value, TimeSpan? expires = null)
        {
            Value = value;
            if (expires != null)
                Expires = Convert.ToInt64(expires.Value.TotalMilliseconds);
        }

        //public DateTime Timestamp { get; } = DateTime.UtcNow;
        public object Value { get; }

        public override bool IsValue => true;
        public long? Expires { get; }

        public override T Match<T>(Func<FalcorValue, T> value, Func<FalcorTree, T> tree)
        {
            throw new NotImplementedException();
        }

        public override JToken ToJToken()
        {
            var result = new JObject {["$type"] = "atom" };
            //result["$timestamp"] = Timestamp.Ticks;
            if (Expires.HasValue) result["$expires"] = Expires;
            //if (Size.HasValue) result["$size"] = Size;
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