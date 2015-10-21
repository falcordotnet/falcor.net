using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing.PathParser
{
    internal sealed class SpracheRouteParser : IRouteParser
    {
        public IReadOnlyList<PathMatcher> Parse(string path) => PathGrammar.RouteMatcher.Parse(path).ToList();
    }
}