using System.Collections.Generic;
using Falcor.Server.Routing;
using Xbehave;
using Xunit;

namespace Falcor.Tests.Server.Routing
{
    public class ResponseBuilderTests
    {
        [Scenario]
        public void Comprehensive()
        {
            "Response builder should produce expected result".x(() =>
            {
                var results = new List<PathValue>();
                var pathValues = new List<PathValue>
                {
                    new PathValue(FalcorPath.From("foo"), "hello"),
                    new PathValue(FalcorPath.From("bar"), "hello"),
                    new PathValue(FalcorPath.From("baz", 1, "first"), "Jessica"),
                    new PathValue(FalcorPath.From("baz", 1, "last"), "Smith"),
                    new PathValue(FalcorPath.From("baz", 1, "phone"), "111-222-1245"),
                    new PathValue(FalcorPath.From("baz", 2, "first"), "Jessica"),
                    new PathValue(FalcorPath.From("baz", 2, "last"), "Smith"),
                    new PathValue(FalcorPath.From("baz", 2, "phone"), "111-222-1245"),
                    new PathValue(FalcorPath.From("baz", 3, "first"), "Jessica"),
                    new PathValue(FalcorPath.From("baz", 3, "last"), "Smith"),
                    new PathValue(FalcorPath.From("baz", 3, "phone"), "111-222-1245")
                };
                var nameHello = new Dictionary<string, object>
                {
                    {"first", "Jessica"},
                    {"last", "Smith"},
                    {"phone", "111-222-1245"}
                };

                var expected = new Dictionary<string, object>
                {
                    {"foo", "hello"},
                    {"bar", "hello"},
                    {
                        "baz", new Dictionary<string, object>
                        {
                            {"1", nameHello},
                            {"2", nameHello},
                            {"3", nameHello}
                        }
                    }
                };
                results.AddRange(pathValues);
                var response = FalcorResponse.From(results);
                Assert.Equal(expected, response.JsonGraph);
            });
        }
    }
}