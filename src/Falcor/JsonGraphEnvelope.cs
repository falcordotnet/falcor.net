using System.Collections.Generic;

namespace Falcor
{
    public class JsonGraphEnvelope
    {
        public JsonGraphEnvelope(object jsonGraph, IEnumerable<FalcorPath> missing, IEnumerable<FalcorPath> paths = null, IEnumerable<FalcorPath> invalidated = null, IEnumerable<FalcorPath> reportedPaths = null)
        {
            JsonGraph = jsonGraph;
            Missing = missing;
            Paths = paths;
            Invalidated = invalidated;
            ReportedPaths = reportedPaths;
        }

        public object JsonGraph { get; }
        public IEnumerable<FalcorPath> Paths { get; }
        public IEnumerable<FalcorPath> Invalidated { get; }
        public IEnumerable<FalcorPath> Missing { get; }
        public IEnumerable<FalcorPath> ReportedPaths { get; }

    }
}