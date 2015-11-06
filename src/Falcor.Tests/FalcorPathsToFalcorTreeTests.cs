using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using static Falcor.Tests.FalcorTestHelpers;

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
