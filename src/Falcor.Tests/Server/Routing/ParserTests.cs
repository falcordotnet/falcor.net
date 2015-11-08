using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcor.Server.Routing.PathParser;
using Sprache;
using Xbehave;
using Xunit;
using static Falcor.Server.Routing.PathParser.PathMatchers;

namespace Falcor.Tests.Server.Routing
{
    public abstract class ParserTests
    {
        public abstract IPathParser Parser { get; }


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
            var path6parsed = Parser.ParseSingle(path6);
            var path6Expected = Path("titlesById", new NumberRange(1, 100), new KeySet("rating", "userRating"));

            var path7 = @"[""genrelist"", [0,1,2], ""name""]";
            var path7Expected = Path("genrelist", new NumericSet(0, 1, 2), "name");

            var path8 = @"[""genrelist"", [""51f2928f34"",""a32e8912f34""], ""name""]";
            var path8Expected = Path("genrelist", new KeySet("51f2928f34", "a32e8912f34"), "name");

            Assert.Equal(path6Expected, path6parsed);


            new List<PathParsingTest>
            {
                Paths(path1).ShouldEqual(path1Expected),
                Paths(path2).ShouldEqual(path2Expected),
                Paths(path3).ShouldEqual(path3Expected),
                Paths(path4).ShouldEqual(path4Expected),
                Paths(path5).ShouldEqual(path5Expected),
                Paths(path7).ShouldEqual(path7Expected),
                Paths(path8).ShouldEqual(path8Expected),
                Paths(path1, path2, path3).ShouldEqual(path1Expected, path2Expected, path3Expected),
                Paths(path4, path1, path2, path3)
                    .ShouldEqual(path4Expected, path1Expected, path2Expected, path3Expected)
            }.ForEach(test =>
            {
                var parsed = Parser.ParseMany(test.Paths);
                Assert.Equal(test.ExpectedPaths, parsed);
            });
        }


        [Scenario]
        public void RouteParsing()
        {
            new List<RouteParsingTest>
            {
                Route("foo").ShouldBehaveLike(StringKey("foo")).ShouldMatch("foo").ShouldFail("bar"),
                Route("foo.bar[{ranges:baz}]")
                    .ShouldBehaveLike(StringKey("foo"), StringKey("bar"),
                        RangesPattern("baz"))
                    .ShouldMatch("foo", "bar", new NumberRange(0, 1))
                    .ShouldFail("bar"),
                Route("foo.bar[{keys}]")
                    .ShouldBehaveLike(StringKey("foo"), StringKey("bar"),
                        KeysPattern("any"))
                    .ShouldMatch("foo", "bar", new KeySet("baz"))
                    .ShouldFail("bar"),
                Route("foo.bar['baz', 'howdy']")
                    .ShouldBehaveLike(StringKey("foo"), StringKey("bar"),
                        KeySet("baz", "howdy"))
                    .ShouldMatch("foo", "bar", "baz")
                    .ShouldMatch("foo", "bar", "howdy")
                    .ShouldFail("bar"),
                Route("foo.bar[{integers}].baz")
                    .ShouldBehaveLike(StringKey("foo"), StringKey("bar"),
                        IntegersPattern(), StringKey("baz"))
                    .ShouldMatch("foo", "bar", new NumericSet(1, 2, 3), "baz")
                    .ShouldMatch("foo", "bar", 1, "baz")
                    .ShouldFail("bar"),
                Route("foo.bar[{integers:ids}]['baz','texas']")
                    .ShouldBehaveLike(StringKey("foo"), StringKey("bar"),
                        IntegersPattern("ids"), KeySet("baz", "texas"))
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

        private static RouteParsingTest Route(string routePath) => new RouteParsingTest { RoutePath = routePath };

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
            return new PathParsingTest { Paths = sb.ToString() };
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
                PathTests.Add(new RouteParsingSubTest { Path = new FalcorPath(keys), ShouldMatch = true });
                return this;
            }

            public RouteParsingTest ShouldFail(params KeySegment[] keys)
            {
                PathTests.Add(new RouteParsingSubTest { Path = new FalcorPath(keys), ShouldMatch = false });
                return this;
            }
        }
    }
}