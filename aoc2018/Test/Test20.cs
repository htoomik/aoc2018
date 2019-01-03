using System.Collections.Generic;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test20
    {
        private readonly ITestOutputHelper _output;
        
        public Test20(ITestOutputHelper output)
        {
            _output = output;
        }

        private const string Input = @"^ENWWW(NEEE|SSE(EE|N))$";

        [Fact]
        public void TestGetPaths()
        {
            var expectedPaths = new List<string>
            {
                "ENWWWNEEE",
                "ENWWWSSEEE",
                "ENWWWSSEN"
            };

            var paths = Day20.GetPaths(Input);
            Assert.Equal(expectedPaths, paths);
        }
        
        [Fact]
        public void TestMap()
        {
            const string expectedMap = @"
#########
#.|.|.|.#
#-#######
#.|.|.|.#
#-#####-#
#.#.#X|.#
#-#-#####
#.|.|.|.#
#########";

            var map = Day20.Map(Input);
            Assert.Equal(expectedMap.Trim(), map.Trim());
        }
    }
}
