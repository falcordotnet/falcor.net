using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Falcor.Server.Routing
{
    public sealed class FalcorResponse
    {
        private FalcorResponse(IDictionary<string, object> jsonGraph, IList<IList<string>> paths)
        {
            JsonGraph = jsonGraph;
            Paths = paths;
        }

        [JsonProperty("jsonGraph")]
        public IDictionary<string, object> JsonGraph { get; }

        [JsonProperty("paths")]
        public IList<IList<string>> Paths { get; }

        public static FalcorResponse From(IReadOnlyList<PathValue> results)
        {
            var jsonGraph = new Dictionary<string, object>();

            foreach (var pathValue in results)
                AddPath(jsonGraph, pathValue.Path, pathValue.Value);

            IList<IList<string>> paths = results
                .Select(pv => pv.Path)
                .Select(path => (IList<string>) path.Select(key => key.ToString()).ToList())
                .ToList();

            var response = new FalcorResponse(jsonGraph, paths);

            return response;
        }

        private static void AddPath(IDictionary<string, object> jsonGraphNode, FalcorPath path, object value)
        {
            var key = path.First().ToString();

            if (path.Count == 1)
            {
                jsonGraphNode[key] = value;
            }
            else
            {
                IDictionary<string, object> nextData;
                if (!jsonGraphNode.ContainsKey(key))
                {
                    nextData = new Dictionary<string, object>();
                    jsonGraphNode.Add(key, nextData);
                }
                else
                {
                    nextData = (IDictionary<string, object>) jsonGraphNode[key];
                }
                AddPath(nextData, path.Skip(1).ToList(), value);
            }
        }
    }
}