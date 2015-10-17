using System;
using System.Collections.Generic;
using System.Linq;
using Falcor.Server;
using Sprache;
using Xbehave;
using Xunit;

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
                //TODO var parsed = RoutingGrammar.KeyMatcher.Parse(input);
                //Assert.Equal("test", parsed);
            });
        }

        [Scenario]
        [Example("foo")]
        [Example("foo bar")]
        [Example(" foo bar baz ")]
        [Example("{foo:bar}")]
        public void CollectionParser(string expectedToken)
        {
            var input = "[" + expectedToken + "]";
            $"Given input \"{input}\" we would expect \"{expectedToken}\" after parsing".x(() =>
            {
                //var parsed = RoutingGrammar.Brackets.Parse(input);
                //Assert.Equal(expectedToken, parsed);
            });
        }
        /* TODO
                [Scenario]
                [Example("{ranges:foo}", "foo", typeof(RangesPatternMatcher))]
                [Example("{ranges}", null, typeof(RangesPatternMatcher))]
                [Example("{integers:bar}", "bar", typeof(IntegersPatternMatcher))]
                [Example("{integers}", null, typeof(IntegersPatternMatcher))]
                [Example("{keys:baz}", "baz", typeof(KeysPatternMatcher))]
                [Example("{keys}", null, typeof(KeysPatternMatcher))]
                public void PatternParser(string input, string name, Type patternType)
                {
                    $"Given a named route patten {input}, we produce a named {patternType.Name}".x(() =>
                    {
                        var output = (IRoutePattern)RoutingGrammar.PatternMatcher.Parse("[" + input + "]");
                        Assert.IsType(patternType, output);
                        if (name != null)
                        {
                            Assert.True(output.IsNamed);
                            Assert.Equal(name, output.Name);
                        }
                        else
                        {
                            Assert.False(output.IsNamed);
                        }
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
                [Example("foo", 0)]
                [Example("foo.bar", 1)]
                [Example("foo.bar[{ranges:baz}]", 2)]
                [Example("foo.bar[{keys}]", 3)]
                [Example("foo.bar['baz', 'howdy']", 4)]
                [Example("foo.bar[{integers}].test", 5)]
                [Example("foo.bar[{integers:ids}]['baz','texas']", 6)]
                public void RouteParsing(string route, int expectedRouteKey)
                {
                    var expectedRoutes = new Dictionary<int, List<PathMatcher>>
                    {

                        { 0, new List<PathMatcher> {
                            "foo"
                        }},
                        { 1, new List<PathMatcher> {
                            "foo",
                            "bar"
                        }},
                        { 2, new List<PathMatcher> {
                            "foo",
                            "bar",
                            new RangesPatternMatcher("baz")
                        }},
                        { 3, new List<PathMatcher> {
                            "foo",
                            "bar",
                            new KeysPatternMatcher()
                        }},
                        { 4, new List<PathMatcher> {
                            "foo",
                            "bar",
                            new KeySetMatcher("baz", "howdy")
                        }},
                        { 5, new List<PathMatcher> {
                            "foo",
                            "bar",
                            new IntegersPatternMatcher(),
                            "test"
                        }},
                        { 6, new List<PathMatcher> {
                            "foo",
                            "bar",
                            new IntegersPatternMatcher("ids"),
                            new KeySetMatcher("baz", "texas")
                        }},

                    };

                    "".x(() =>
                    {
                        var expected = expectedRoutes[expectedRouteKey];
                        var parsed = RoutingGrammar.Action.Parse(route).ToList();
                        Assert.True(expected.SequenceEqual(parsed));
                    });

                }

            */
    }
}
