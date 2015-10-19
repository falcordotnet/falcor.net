using System.Collections.Generic;

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
    }
}