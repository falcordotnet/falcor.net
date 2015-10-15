using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace Falcor.Tests
{
    public class Class1
    {
        [Scenario]
        public void Test(int x, int y, int result)
        {
            "Given the number 2".x(() => x = 2);
            "And the number 3".x(() => y = 3);
            "Add the two numbers together".x(() => result = x + y);
            "The result should be 5"
                .x(() => Assert.Equal(5, result));
        }

        [Fact]
        public void AssertTruth()
        {
            Assert.True(true);
        }
    }
}
