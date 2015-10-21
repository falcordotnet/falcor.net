using System.Collections.Generic;

namespace Falcor.Server.Routing.PathParser
{
    internal sealed class MemoizedRouteParser : IRouteParser
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
}