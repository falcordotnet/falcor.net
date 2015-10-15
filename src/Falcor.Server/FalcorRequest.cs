using System.Collections.Generic;

namespace Falcor.Server
{
    public class FalcorRequest<TRequest>
    {
        public FalcorRequest(FalcorMethod method, TRequest request, ICollection<FalcorPath> paths)
        {
            Method = method;
            Request = request;
            Paths = paths;
        }

        public TRequest Request { get; }
        public FalcorMethod Method { get; }
        public ICollection<FalcorPath> Paths { get; }
    }
}