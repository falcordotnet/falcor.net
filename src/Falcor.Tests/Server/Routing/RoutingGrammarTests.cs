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
        public void StringKey(string routePath)
        {
            "".x(() =>
            {
                var matcher = RoutingGrammar.StringKeyMatcher.Parse(routePath);
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

        [Scenario]
        public void RouteParsing()
        {
            new List<PathMatcherTest>()
            {
                Route("foo").ShouldBehaveLike(PathMatchers.StringKey("foo")).ShouldMatch("foo").ShouldFail("bar"),

                Route("foo.bar[{ranges:baz}]")
                .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"), RangesPattern("baz"))
                .ShouldMatch("foo", "bar", new NumberRange(0, 1))
                .ShouldFail("bar"),

                Route("foo.bar[{keys}]")
                .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"), KeysPattern("any"))
                .ShouldMatch("foo", "bar", new KeySet("baz"))
                .ShouldFail("bar"),

                Route("foo.bar['baz', 'howdy']")
                .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"), KeySet("baz", "howdy"))
                .ShouldMatch("foo", "bar", "baz")
                .ShouldMatch("foo", "bar", "howdy")
                .ShouldFail("bar"),

                Route("foo.bar[{integers}].baz")
                .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"), IntegersPattern(), PathMatchers.StringKey("baz"))
                .ShouldMatch("foo", "bar", new NumericSet(1, 2, 3), "baz")
                .ShouldMatch("foo", "bar", 1, "baz")
                .ShouldFail("bar"),

                Route("foo.bar[{integers:ids}]['baz','texas']")
                .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"), IntegersPattern("ids"), KeySet("baz", "texas"))
                .ShouldMatch("foo", "bar", 1, "baz")
                .ShouldMatch("foo", "bar", 1, "texas")
                .ShouldFail("bar")
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
                        Assert.True(outputResult == expectedResult);
                        Assert.True(outputResult == testPath.ShouldMatch);
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

            public PathMatcherTest ShouldMatch(params KeySegment[] keys)
            {
                PathTests.Add(new PathTest() { Path = new FalcorPath(keys), ShouldMatch = true });
                return this;
            }

            public PathMatcherTest ShouldFail(params KeySegment[] keys)
            {
                PathTests.Add(new PathTest() { Path = new FalcorPath(keys), ShouldMatch = false });
                return this;
            }
        }

        private static PathMatcherTest Route(string routePath) => new PathMatcherTest { RoutePath = routePath };
    }
}
