using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing;
using Sprache;
using Xbehave;
using Xunit;
using static Falcor.Server.Routing.PathMatchers;

namespace Falcor.Tests.Server
{
    public class RoutingGrammarTests
    {
        [Scenario]
        [Example("test")]
        [Example("test[foo]")]
        [Example("test.bar")]
        public void IdentifierParser(string input)
        {
            "".x(() =>
            {
                var matcher = RoutingGrammar.StringKeyMatcher.Parse(input);
                Assert.True(matcher("test").IsMatched);
            });
        }


        [Scenario]
        public void PatternParser()
        {
            new Dictionary<string, KeySegment>
            {
                {"{ranges}", new NumberRange(0, 10)},
                {"{ranges:foo}", new NumberRange(0, 10)},
                {"{integers}", new NumericSet(0)},
                {"{integers:foo}", new NumericSet(0)},
                {"{keys}", new KeySet("foo")},
                {"{keys:foo}", new KeySet("foo", "bar")}

            }.Select((kv, index) => new { Pattern = kv.Key, KeySegment = kv.Value, ExpectName = (index % 2 != 0) })
            .ToList()
            .ForEach(test =>
            {
                $"Given a route pattern '{test.Pattern}' we can expect a match on a '{test.KeySegment}'".x(() =>
                {
                    var matcher = RoutingGrammar.PatternMatcher.Parse("[" + test.Pattern + "]");
                    var result = matcher(test.KeySegment);
                    if (test.ExpectName)
                        Assert.True(result.HasName && result.Name == "foo");

                    Assert.True(result.IsMatched);
                    Assert.True(result.HasValue);
                    Assert.Equal(test.KeySegment, result.Value);
                });
            });
        }

        [Scenario]
        public void BasicParsers()
        {
            "single quoted string".x(() =>
            {
                var expected = "howdydoody";
                var input = $"'{expected}'";
                var output = RoutingGrammar.SingleQuotedString.Parse(input);
                Assert.Equal(expected, output);
            });

            "double quoted string".x(() =>
            {
                var expected = "howdydoody";
                var input = $"\"{expected}\"";
                var output = RoutingGrammar.DoubleQuotedString.Token().Parse(input);
                Assert.Equal(expected, output);
            });

            "Comma seperated single quoted strings".x(() =>
            {
                var input = "   'foo', 'bar','baz'   ";
                var output = RoutingGrammar.CommaSeperatedSingleQuotedStrings.Parse(input);
                var expected = new[] { "foo", "bar", "baz" };
                Assert.Equal(expected, output);
            });
        }

        private List<PathMatcher> Matchers(params PathMatcher[] matchers) => matchers.ToList();

        private class PathTest
        {
            public FalcorPath Path { get; set; }
            public bool ShouldMatch { get; set; }
        }
        private class PathMatcherTest
        {
            public string RoutePath { get; set; }
            public List<PathTest> PathTests { get; set; } = new List<PathTest>();
            public List<PathMatcher> ExpectedMatchers { get; set; }

            public PathMatcherTest ShouldBehaveLike(params PathMatcher[] pathMatchers)
            {
                ExpectedMatchers = pathMatchers.ToList();
                return this;
            }

            public PathMatcherTest WhenMatching(params KeySegment[] keys)
            {
                PathTests.Add(new PathTest() { Path = new FalcorPath(keys), ShouldMatch = true });
                return this;
            }

            public PathMatcherTest WhenNotMatching(params KeySegment[] keys)
            {
                PathTests.Add(new PathTest() { Path = new FalcorPath(keys), ShouldMatch = false });
                return this;
            }
        }

        private static PathMatcherTest Route(string routePath) => new PathMatcherTest() { RoutePath = routePath };

        [Scenario]
        public void RouteParsing()
        {
            //[Example("foo.bar", 1)]
            //[Example("foo.bar[{ranges:baz}]", 2)]
            //[Example("foo.bar[{keys}]", 3)]
            //[Example("foo.bar['baz', 'howdy']", 4)]
            //[Example("foo.bar[{integers}].test", 5)]
            //[Example("foo.bar[{integers:ids}]['baz','texas']", 6)] 
            new List<PathMatcherTest>()
            {
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar"),
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar"),
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar"),
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar"),
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar"),
                Route("foo").ShouldBehaveLike(StringKey("foo")).WhenMatching("foo").WhenNotMatching("bar")
            }
            .ForEach(test =>
            {
                "".x(() =>
                {
                    var output = RoutingGrammar.Route.Parse(test.RoutePath).ToList();
                    foreach (var testPath in test.PathTests)
                    {
                        var outputResult = IsMatch(output, testPath.Path);
                        var expectedResult = IsMatch(test.ExpectedMatchers, testPath.Path);
                        Assert.True(testPath.ShouldMatch == outputResult == expectedResult);
                    }
                });
            });
        }

        private bool IsMatch(List<PathMatcher> matchers, FalcorPath path)
        {
            for (int index = 0; index < matchers.Count; index++)
            {
                var result = matchers[index](path[index]);
                if (!result.IsMatched) return false;
            }
            return true;
        }
    }
}
