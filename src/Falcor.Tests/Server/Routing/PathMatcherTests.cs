using System.Collections.Generic;
using Falcor.Server.Routing;
using Falcor.Server.Routing.PathParser;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class PathMatcherTests
    {
        [Scenario]
        public void KeySetMatcher()
        {
            var keySetMatcher = PathMatchers.KeySet("foo", "bar");
            new List<MatchResult>()
            {
                keySetMatcher(new KeySet("foo", "bar")),
                keySetMatcher(new KeySet("foo")),
                keySetMatcher(new KeySet("bar")),
                keySetMatcher("foo"),
                keySetMatcher("bar")
            }.ForEach(test => Assert.True(test.IsMatched));
        }
    }
}