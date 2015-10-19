using System.Collections.Generic;
using System.Linq;

namespace Falcor.Server.Routing
{
    public class FalcorRequest
    {
        public FalcorRequest(FalcorMethod method, IReadOnlyList<FalcorPath> paths)
        {
            Method = method;
            Paths = paths;
        }

        public FalcorMethod Method { get; }
        public IReadOnlyList<FalcorPath> Paths { get; }

        public static FalcorRequest Get(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Get, paths);
        public static FalcorRequest Get(params KeySegment[] keys) => Get(new FalcorPath(keys));
        //public static FalcorRequest Set(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Get, paths);
        //public static FalcorRequest Call(params FalcorPath[] paths) => new FalcorRequest(FalcorMethod.Get, paths);
    }
}