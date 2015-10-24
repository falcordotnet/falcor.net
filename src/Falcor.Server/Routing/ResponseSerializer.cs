using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Falcor.Server.Routing
{
    internal sealed class ResponseSerializer : IResponseSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public ResponseSerializer()
        {
            _jsonSerializer = new JsonSerializer();
        }

        public string Serialize(FalcorResponse response)
        {
            var result = new JObject();
            result["jsonGraph"] = SerializeItem(response.JsonGraph);
            var stringWriter = new StringWriter();
            _jsonSerializer.Serialize(stringWriter, result);
            return stringWriter.ToString();
        }

        private static JToken SerializeItem(object value)
        {
            if (value == null) return null;

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
            if (array != null) return new JArray(array.Select(SerializeItem).ToArray());

            return new JObject();
        }

        private static JToken SerializeRef(Ref reference)
        {
            var result = new JObject();
            result["$type"] = "ref";
            result["value"] = new JArray(reference.AsRef().Select(SerializeItem).ToList());
            return result;
        }

        private static JToken SerializeError(Error error)
        {
            var result = new JObject();
            result["$type"] = "error";
            var value = new JObject();
            value["message"] = error.ToString();
            result["value"] = value;
            return result;
        }
    }
}