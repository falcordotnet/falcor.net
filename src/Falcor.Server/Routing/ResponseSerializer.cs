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
            result["jsonGraph"] = SerializationHelper.SerializeItem(response.JsonGraph);
            var stringWriter = new StringWriter();
            _jsonSerializer.Serialize(stringWriter, result);
            return stringWriter.ToString();
        }

        private static JToken SerializeRef(Ref reference)
        {
            var result = new JObject();
            result["$type"] = "ref";
            result["value"] = new JArray(reference.AsRef().Select(SerializationHelper.SerializeItem).ToList());
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