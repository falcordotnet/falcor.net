using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcor.Server.Routing.PathParser;
using Sprache;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class ParserTests
    {
        [Scenario]
        [Example("test")]
        [Example("test[foo]")]
        [Example("test.bar")]
        public void StringKey(string routePath)
        {
            "".x(() =>
            {
                var matcher = PathGrammar.StringKeyMatcher.Parse(routePath);
                Assert.True(matcher("test").IsMatched);
            });
        }

        [Scenario]
        public void SingleQuotedParser() =>
            Assert.Equal("test", Parse.String("test").Text().SingleQuoted().Parse("'test'"));

        [Scenario]
        public void DoubleQuotedParser() =>
            Assert.Equal("test", PathGrammar.Text("test").DoubleQuoted().Parse("\"test\""));

        [Scenario]
        public void Bracketed() =>
            Assert.Equal("foo", PathGrammar.Text("foo").Bracketed().Parse("[foo]"));

        [Scenario]
        public void Braced() =>
            Assert.Equal("foo", PathGrammar.Text("foo").Braced().Parse("{foo}"));

        [Scenario]
        [Example(1, "1")]
        [Example(55, "55")]
        [Example(0, "0")]
        public void Number(int expected, string input)
        {
            Assert.Equal(expected, PathGrammar.Number.Parse(input));
            Assert.Equal(new NumberKey(expected), PathGrammar.NumberKey.Parse(input));
        }

        [Scenario]
        [Example(@"{ ""from"":1,""to"":2}", 1, 2)]
        [Example(@"{""from"":10,""to"":15}", 10, 15)]
        [Example(@"{ ""from"":0,""to"":22}", 0, 22)]
        public void NumberRange(string input, int from, int to) =>
            Assert.Equal(new NumberRange(from, to), PathGrammar.NumberRange.Parse(input));

        [Scenario]
        [Example("true", true)]
        [Example("false", false)]
        public void Boolean(string input, bool expected)
        {
            Assert.Equal(expected, PathGrammar.Boolean.Parse(input));
            Assert.Equal(new BooleanKey(expected), PathGrammar.BooleanKey.Parse(input));
        }

        [Scenario]
        public void Key()
        {
            new Dictionary<string, SimpleKey>
            {
                {"true", true},
                {"false", false},
                {@"""howdy""", new StringKey("howdy")},
                {"null", new NullKey()}
            }.Select(kv => new {kv.Key, Expected = kv.Value})
                .ToList().ForEach(test =>
                {
                    var output = PathGrammar.Key.Parse(test.Key);
                    Assert.Equal(test.Expected, output);
                });
        }

        [Scenario]
        [Example(@"""foo""", "foo")]
        [Example(@"""bar""", "bar")]
        [Example(@"""baz""", "baz")]
        public void StringKey(string input, string expected) =>
            Assert.Equal(new StringKey(expected), PathGrammar.StringKey.Parse(input));


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
            }.Select((kv, index) => new {Pattern = kv.Key, KeySegment = kv.Value, ExpectName = (index%2 != 0)})
                .ToList()
                .ForEach(test =>
                {
                    $"Given a route pattern '{test.Pattern}' we can expect a match on a '{test.KeySegment}'".x(() =>
                    {
                        var matcher = PathGrammar.PatternMatcher.Parse("[" + test.Pattern + "]");
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
                var output = PathGrammar.SingleQuotedString.Parse(input);
                Assert.Equal(expected, output);
            });

            "double quoted string".x(() =>
            {
                var expected = "howdydoody";
                var input = $"\"{expected}\"";
                var output = PathGrammar.DoubleQuotedString.Token().Parse(input);
                Assert.Equal(expected, output);
            });

            "Comma seperated single quoted strings".x(() =>
            {
                var input = "   'foo', \"bar\",'baz'   ";
                var output = PathGrammar.CommaSeperatedQuotedStrings.Parse(input);
                var expected = new[] {"foo", "bar", "baz"};
                Assert.Equal(expected, output);
            });
        }

        [Scenario]
        public void PathParsing()
        {
            var path1 = @"[""foo""]";
            var path1Expected = Path("foo");
            var path2 = @"[""foo"",null]";
            var path2Expected = Path("foo", NullKey.Instance);
            var path3 = @"[""foo"", {""from"":1, ""to"":2 }]";
            var path3Expected = Path("foo", new NumberRange(1, 2));
            var path4 = @"[""foo"", [""bar"", ""baz""]]";
            var path4Expected = Path("foo", new KeySet("bar", "baz"));
            var path5 = @"[""foo"", {""from"":1, ""to"":2 }, [""bar"", ""baz""]]";
            var path5Expected = Path("foo", new NumberRange(1, 2), new KeySet("bar", "baz"));
            var path6 = @"[""titlesById"",{""from"":1,""to"":100},[""rating"",""userRating""]]";
            var path6parsed = PathGrammar.Path.Parse(path6);
            var path6Expected = Path("titlesById", new NumberRange(1, 100), new KeySet("rating", "userRating"));

            var path7 = @"[""genrelist"", [0,1,2], ""name""]";
            var path7Expected = Path("genrelist", new NumericSet(0, 1, 2), "name");
            Assert.Equal(path6Expected, path6parsed);


            new List<PathParsingTest>
            {
                Paths(path1).ShouldEqual(path1Expected),
                Paths(path2).ShouldEqual(path2Expected),
                Paths(path3).ShouldEqual(path3Expected),
                Paths(path4).ShouldEqual(path4Expected),
                Paths(path5).ShouldEqual(path5Expected),
                Paths(path7).ShouldEqual(path7Expected),
                Paths(path1, path2, path3).ShouldEqual(path1Expected, path2Expected, path3Expected),
                Paths(path4, path1, path2, path3)
                    .ShouldEqual(path4Expected, path1Expected, path2Expected, path3Expected)
            }.ForEach(test =>
            {
                var parsed = PathGrammar.Paths.Parse(test.Paths);
                Assert.Equal(test.ExpectedPaths, parsed);
            });
        }

        [Scenario]
        public void NumericSet()
        {
            new Dictionary<string, NumericSet>
            {
                {"[1,2,3]", new NumericSet(1, 2, 3)},
                {"[1]", new NumericSet(1)},
                {"[  1, 2, 3]", new NumericSet(1, 2, 3)},
                {"[1,2,3, 4]", new NumericSet(1, 2, 3, 4)}
            }.Select(kv => new {Input = kv.Key, Expected = kv.Value})
                .ToList().ForEach(test =>
                {
                    var output = PathGrammar.NumericSet.Parse(test.Input);
                    Assert.Equal(test.Expected, output);
                });
        }

        [Scenario]
        public void RouteParsing()
        {
            new List<RouteParsingTest>
            {
                Route("foo").ShouldBehaveLike(PathMatchers.StringKey("foo")).ShouldMatch("foo").ShouldFail("bar"),
                Route("foo.bar[{ranges:baz}]")
                    .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"),
                        PathMatchers.RangesPattern("baz"))
                    .ShouldMatch("foo", "bar", new NumberRange(0, 1))
                    .ShouldFail("bar"),
                Route("foo.bar[{keys}]")
                    .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"),
                        PathMatchers.KeysPattern("any"))
                    .ShouldMatch("foo", "bar", new KeySet("baz"))
                    .ShouldFail("bar"),
                Route("foo.bar['baz', 'howdy']")
                    .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"),
                        PathMatchers.KeySet("baz", "howdy"))
                    .ShouldMatch("foo", "bar", "baz")
                    .ShouldMatch("foo", "bar", "howdy")
                    .ShouldFail("bar"),
                Route("foo.bar[{integers}].baz")
                    .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"),
                        PathMatchers.IntegersPattern(), PathMatchers.StringKey("baz"))
                    .ShouldMatch("foo", "bar", new NumericSet(1, 2, 3), "baz")
                    .ShouldMatch("foo", "bar", 1, "baz")
                    .ShouldFail("bar"),
                Route("foo.bar[{integers:ids}]['baz','texas']")
                    .ShouldBehaveLike(PathMatchers.StringKey("foo"), PathMatchers.StringKey("bar"),
                        PathMatchers.IntegersPattern("ids"), PathMatchers.KeySet("baz", "texas"))
                    .ShouldMatch("foo", "bar", 1, "baz")
                    .ShouldMatch("foo", "bar", 1, "texas")
                    .ShouldFail("bar")
            }
                .ForEach(test =>
                {
                    "".x(() =>
                    {
                        var output = PathGrammar.RouteMatcher.Parse(test.RoutePath).ToList();
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
            for (var index = 0; index < matchers.Count; index++)
            {
                var result = matchers[index](path[index]);
                if (!result.IsMatched) return false;
            }
            return true;
        }

        private static RouteParsingTest Route(string routePath) => new RouteParsingTest {RoutePath = routePath};

        private static PathParsingTest Paths(params string[] paths)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            for (var index = 0; index < paths.Length; index++)
            {
                var path = paths[index];
                sb.Append(path);
                if (index + 1 < paths.Length)
                    sb.Append(", ");
            }
            sb.Append("]");
            return new PathParsingTest {Paths = sb.ToString()};
        }

        private static FalcorPath Path(params KeySegment[] keys) => new FalcorPath(keys);

        private class PathParsingTest
        {
            public string Paths { get; set; }
            public List<FalcorPath> ExpectedPaths { get; set; }

            public PathParsingTest ShouldEqual(params FalcorPath[] paths)
            {
                ExpectedPaths = paths.ToList();
                return this;
            }
        }

        private class PathParsingSubTest
        {
            public List<FalcorPath> Paths { get; } = new List<FalcorPath>();
            public bool ShouldMatch { get; set; }
        }


        private class RouteParsingSubTest
        {
            public FalcorPath Path { get; set; }
            public bool ShouldMatch { get; set; }
        }

        private class RouteParsingTest
        {
            public string RoutePath { get; set; }
            public List<RouteParsingSubTest> PathTests { get; } = new List<RouteParsingSubTest>();
            public List<PathMatcher> ExpectedMatchers { get; set; }

            public RouteParsingTest ShouldBehaveLike(params PathMatcher[] pathMatchers)
            {
                ExpectedMatchers = pathMatchers.ToList();
                return this;
            }

            public RouteParsingTest ShouldMatch(params KeySegment[] keys)
            {
                PathTests.Add(new RouteParsingSubTest {Path = new FalcorPath(keys), ShouldMatch = true});
                return this;
            }

            public RouteParsingTest ShouldFail(params KeySegment[] keys)
            {
                PathTests.Add(new RouteParsingSubTest {Path = new FalcorPath(keys), ShouldMatch = false});
                return this;
            }
        }
    }
}