using System.Collections.Generic;
using Newtonsoft.Json;

namespace Falcor.Server.Routing
{
    public sealed class FalcorResponse
    {
        public FalcorResponse(IDictionary<string, object> jsonGraph, IList<IList<string>> paths)
        {
            JsonGraph = jsonGraph;
            Paths = paths;
        }

        [JsonProperty("jsonGraph")]
        public IDictionary<string, object> JsonGraph { get; }

        [JsonProperty("paths")]
        public IList<IList<string>> Paths { get; }
    }
}