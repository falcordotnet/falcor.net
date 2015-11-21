using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public sealed class FalcorRequest
    {
        public FalcorRequest(FalcorMethod method, IReadOnlyList<FalcorPath> paths) : this(method, paths, null)
        {
        }

        public FalcorRequest(FalcorMethod method, IReadOnlyList<FalcorPath> paths, dynamic jsonGraph)
        {
            Method = method;
            Paths = paths;
            JsonGraph = jsonGraph;
        }

        public FalcorMethod Method { get; }
        public IReadOnlyList<FalcorPath> Paths { get; }
        public dynamic JsonGraph { get; }

        public static FalcorRequest Get(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Get, paths);
        public static FalcorRequest Get(params KeySegment[] keys) => Get(FalcorPath.Create(keys));
        public static FalcorRequest Set(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Set, paths);
        public static FalcorRequest Set(params KeySegment[] keys) => Set(FalcorPath.Create(keys));
        public static FalcorRequest Call(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Call, paths);
        public static FalcorRequest Call(params KeySegment[] keys) => Call(FalcorPath.Create(keys));
    }
}