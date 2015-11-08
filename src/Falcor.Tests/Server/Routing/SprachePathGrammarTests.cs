using System.Collections.Generic;
using System.Linq;
using Falcor.Server.Routing.PathParser;
using Sprache;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class SprachePathGrammarTests
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
            }.Select(kv => new { kv.Key, Expected = kv.Value })
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
        [Example(@"""51f2928f34""", "51f2928f34")]
        [Example(@"""9bdc2705-75d8-4ae0-9a9a-fd7d47b75113""", "9bdc2705-75d8-4ae0-9a9a-fd7d47b75113")]
        [Example(@"""1""", "1")]
        [Example(@""" 1 """, " 1 ")]
        [Example("\"\tfoo\t\"", "\tfoo\t")]
        public void StringKeyParser(string input, string expected) =>
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
            }.Select((kv, index) => new { Pattern = kv.Key, KeySegment = kv.Value, ExpectName = index % 2 != 0 })
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
                var expected = new[] { "foo", "bar", "baz" };
                Assert.Equal(expected, output);
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
            }.Select(kv => new { Input = kv.Key, Expected = kv.Value })
                .ToList().ForEach(test =>
                {
                    var output = PathGrammar.NumericSet.Parse(test.Input);
                    Assert.Equal(test.Expected, output);
                });
        }
    }
}