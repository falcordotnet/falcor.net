using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Falcor
{
    internal static class SerializationHelper
    {
        public static JToken SerializeItem(object value)
        {
            var falcorValueOrKey = value as IJson;
            if (falcorValueOrKey != null) return ((IJson)value).ToJson();

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