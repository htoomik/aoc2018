using System.IO;
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

            var map = new Day20().Map(Input);
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

            var map = new Day20().Map(input);
            Assert.Equal(expectedMap.Trim(), map.Trim());
        }

        [Fact]
        public void TestMap_3()
        {
            const string input = @"^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";
            const string expectedMap = @"
#############
#.|.|.|.|.|.#
#-#####-###-#
#.#.|.#.#.#.#
#-#-###-#-#-#
#.#.#.|.#.|.#
#-#-#-#####-#
#.#.#.#X|.#.#
#-#-#-###-#-#
#.|.#.|.#.#.#
###-#-###-#-#
#.|.#.|.|.#.#
#############";

            var map = new Day20().Map(input);
            Assert.Equal(expectedMap.Trim(), map.Trim());
        }

        [Theory]
        [InlineData("^WNE$", 3)]
        [InlineData("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
        [InlineData("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23)]
        [InlineData("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31)]
        public void TestSolve(string input, int expected)
        {
            var result = new Day20().Solve(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input20.txt");
            var result = new Day20().Solve(input.Trim());
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input20.txt");
            var result = new Day20().Solve2(input.Trim());
            _output.WriteLine(result.ToString());
        }
    }
}
