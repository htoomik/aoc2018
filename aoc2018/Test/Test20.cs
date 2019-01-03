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
        
        [Fact]
        public void TestMap_2()
        {
            const string input = @"^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            const string expectedMap = @"
###########
#.|.#.|.#.#
#-###-#-#-#
#.|.|.#.#.#
#-#####-#-#
#.#.#X|.#.#
#-#-#####-#
#.#.|.|.|.#
#-###-###-#
#.|.|.#.|.#
###########";

            var map = Day20.Map(input);
            Assert.Equal(expectedMap.Trim(), map.Trim());
        }

        [Theory]
        [InlineData("^WNE$", 3)]
        [InlineData("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
        [InlineData("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23)]
        [InlineData("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31)]
        public void TestSolve(string input, int expected)
        {
            var result = Day20.Solve(input);
            Assert.Equal(expected, result);
        }
    }
}
