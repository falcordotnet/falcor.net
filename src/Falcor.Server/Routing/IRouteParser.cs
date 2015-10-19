using System.Collections.Generic;

namespace Falcor.Server.Routing
{
    public interface IRouteParser
    {
        List<PathMatcher> Parse(string path);
    }
}