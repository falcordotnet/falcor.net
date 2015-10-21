using System.Collections.Generic;

namespace Falcor.Server.Routing.PathParser
{
    public interface IRouteParser
    {
        IReadOnlyList<PathMatcher> Parse(string path);
    }
}