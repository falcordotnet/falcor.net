using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    internal sealed class FalcorResponseBuilder
    {
        private readonly List<PathValue> _pathValues = new List<PathValue>();

        public bool Contains(FalcorPath path) => _pathValues.Any(pv => pv.Path.Equals(path));
        public void Add(FalcorPath path, object value) => _pathValues.Add(new PathValue(path, value));
        public void AddRange(IEnumerable<PathValue> pathValues) => _pathValues.AddRange(pathValues);

        public FalcorResponse CreateResponse()
        {
            var jsonGraph = new Dictionary<string, object>();

            foreach (var pathValue in _pathValues)
                AddPath(jsonGraph, pathValue.Path, pathValue.Value);

            IList<IList<string>> paths = _pathValues
                .Select(pv => pv.Path)
                .Select(path => (IList<string>)path.Select(key => key.ToString()).ToList())
                .ToList();

            var response = new FalcorResponse(jsonGraph, paths);

            return response;
        }

        private void AddPath(IDictionary<string, object> jsonGraphNode, FalcorPath path, object value)
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
                    nextData = (IDictionary<string, object>)jsonGraphNode[key];
                }
                AddPath(nextData, path.Skip(1).ToList(), value);
            }
        }
    }
}