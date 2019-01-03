using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test18
    {
        private readonly ITestOutputHelper _output;

        private const string Input = @"
.#.#...|#.
.....#|##|
.|..|...#.
..|#.....#
#.#|||#|#|
...#.||...
.|....|...
||...#|.#|
|.||||..|.
...#.|..|.";

        private const string Expected1 = @"
.......##.
......|###
.|..|...#.
..|#||...#
..##||.|#|
...#||||..
||...|||..
|||||.||.|
||||||||||
....||..|.";

        private const string Expected10 = @"
.||##.....
||###.....
||##......
|##.....##
|##.....##
|##....##|
||##.####|
||#####|||
||||#|||||
||||||||||";

        public Test18(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var result = Day18.Solve(Input, 1);
            Assert.Equal(Expected1.Trim(), result.state);
        }

        [Fact]
        public void Test10()
        {
            var result = Day18.Solve(Input, 10);
            Assert.Equal(Expected10.Trim(), result.state);
            Assert.Equal(1147, result.answer);
        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input18.txt");
            var result = Day18.Solve(input, 10);
            _output.WriteLine(result.answer.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input18.txt");
            var result = Day18.Solve(input, 1100);
            _output.WriteLine(result.answer.ToString());
            _output.WriteLine(result.repeat.ToString());

            var cycle = result.repeat - 1000;
            var remainder = (1000000000 - 1000) % cycle;
            var finalResult = Day18.Solve(input, 1000 + remainder);
            _output.WriteLine(finalResult.answer.ToString());
        }
    }
}
