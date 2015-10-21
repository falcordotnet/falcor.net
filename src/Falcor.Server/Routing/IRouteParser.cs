using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IRouteParser
    {
        IReadOnlyList<PathMatcher> Parse(string path);
    }

    public interface IPathParser
    {
        IReadOnlyList<FalcorPath> Parse(string paths);
    }
}