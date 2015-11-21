using System.Collections.Generic;
using Xbehave;

namespace Falcor.Tests
{
    public class FalcorPathsToFalcorTreeTests
    {
        [Scenario]
        public void ToPaths()
        {
            "toPaths a pathmap that has overlapping branch and leaf nodes".x(() =>
            {
                var pathMaps = new List<IDictionary<SimpleKey, FalcorNode>>
                {
                    new Dictionary<SimpleKey, FalcorNode>
                    {
                        { "lolomo", 1 }
                    }
                };
            });
        }
    }
}
