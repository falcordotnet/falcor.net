using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing
{
    internal class MemoizedRouteParser : IRouteParser
    {
        private static readonly Dictionary<string, List<PathMatcher>> RouteParsingCache = new Dictionary<string, List<PathMatcher>>();
        private readonly IRouteParser _innerParser;

        public MemoizedRouteParser(IRouteParser innerParser)
        {
            _innerParser = innerParser;
        }

        public List<PathMatcher> Parse(string path)
        {
            List<PathMatcher> pathMatchers;
            if (!RouteParsingCache.TryGetValue(path, out pathMatchers))
                RouteParsingCache[path] = pathMatchers = _innerParser.Parse(path);
            return pathMatchers;
        }
    }

    internal class SpracheRouteParser : IRouteParser
    {
        public List<PathMatcher> Parse(string path) => RoutingGrammar.Route.Parse(path).ToList();
    }
}