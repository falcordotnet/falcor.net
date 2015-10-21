using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Falcor.Server.Routing
{
    internal class MemoizedRouteParser : IRouteParser
    {
        private static readonly Dictionary<string, IReadOnlyList<PathMatcher>> RouteParsingCache = new Dictionary<string, IReadOnlyList<PathMatcher>>();
        private readonly IRouteParser _innerParser;

        public MemoizedRouteParser(IRouteParser innerParser)
        {
            _innerParser = innerParser;
        }

        public IReadOnlyList<PathMatcher> Parse(string path)
        {
            IReadOnlyList<PathMatcher> pathMatchers;
            if (!RouteParsingCache.TryGetValue(path, out pathMatchers))
                RouteParsingCache[path] = pathMatchers = _innerParser.Parse(path);
            return pathMatchers;
        }
    }

    internal class SpracheRouteParser : IRouteParser
    {
        public IReadOnlyList<PathMatcher> Parse(string path) => PathParsingGrammar.RouteMatcher.Parse(path).ToList();
    }

    internal class SprachePathParser : IPathParser
    {
        public IReadOnlyList<FalcorPath> Parse(string paths)
        {
            throw new System.NotImplementedException();
        }
    }
}