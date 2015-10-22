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

        private JToken SerializeItem(object value)
        {
            if (value is int)
            {
                return new JValue((int)value);
            }

            var stringValue = value as string;
            if (stringValue != null)
            {
                return new JValue(stringValue);
            }

            var reference = value as Ref;
            if (reference != null)
            {
                return SerializeRef(reference);
            }

            var error = value as Error;
            if (error != null) return SerializeError(error);

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
            if (array != null)
            {
                return new JArray(array.Select(SerializeItem).ToArray());
            }


            return new JObject();
        }

        private JToken SerializeRef(Ref reference)
        {
            var result = new JObject();
            result["$type"] = "ref";
            result["value"] = new JArray(reference.AsRef().ToList());
            return result;
        }

        private JToken SerializeError(Error error)
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