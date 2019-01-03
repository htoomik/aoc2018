using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test17
    {
        private readonly ITestOutputHelper _output;

        #region Inputs
        private const string Input = @"
x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504";

        private const string Map = @"
......+.......
............#.
.#..#.......#.
.#..#..#......
.#..#..#......
.#.....#......
.#.....#......
.#######......
..............
..............
....#.....#...
....#.....#...
....#.....#...
....#######...";

        private const string Expected = @"
......+.......
......|.....#.
.#..#||||...#.
.#..#~~#|.....
.#..#~~#|.....
.#~~~~~#|.....
.#~~~~~#|.....
.#######|.....
........|.....
...|||||||||..
...|#~~~~~#|..
...|#~~~~~#|..
...|#~~~~~#|..
...|#######|..";

        private const int Count = 57;

        private const string Map2 = @"
......+..........
.................
.................
..#.........#....
..#.........#....
..#...#..#..#....
..#...####..#....
..#.........#....
..#.........#....
..###########....";
        
        private const string Expected2 = @"
......+..........
......|..........
.|||||||||||||...
.|#~~~~~~~~~#|...
.|#~~~~~~~~~#|...
.|#~~~#~~#~~#|...
.|#~~~####~~#|...
.|#~~~~~~~~~#|...
.|#~~~~~~~~~#|...
.|###########|...";

        private const int Count2 = 4 * 9 + 7 + 5 + 2 * 7;

        private const string Map3 = @"
.....+.......
.............
.............
.#.........#.
.#.........#.
.#...#..#..#.
.#...####..#.
.#.........#.
.#.........#.
.###########.";
        
        private const string Expected3 = @"
.....+.......
.....|.......
|||||||||||||
|#~~~~~~~~~#|
|#~~~~~~~~~#|
|#~~~#~~#~~#|
|#~~~####~~#|
|#~~~~~~~~~#|
|#~~~~~~~~~#|
|###########|";

        private const int Count3 = 62;

        #endregion

        public Test17(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Parse()
        {
            
            var (scan, _) = Day17.Parse(Input.Trim());
            var map = Day17.Print(scan);
            Assert.Equal(Map.Trim(), map.Trim());
        }

        [Fact]
        public void Test()
        {
            var (scan, minY) = Day17.Parse(Input.Trim());
            var wateredScan = new Day17().Pour(scan);
            var (count1, _) = Day17.CountWater(scan, minY);
            var wateredMap = Day17.Print(wateredScan);
            Assert.Equal(Expected.Trim(), wateredMap.Trim());
            Assert.Equal(Count, count1);
        }

        [Theory]
        [InlineData(Map2, Expected2, Count2)]
        [InlineData(Map3, Expected3, Count3)]
        public void TestSpecialCases(string map, string expected, int expectedCount)
        {
            var lines = map.Trim().Split("\r\n");
            var scan = new char[lines.Length, lines[0].Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    scan[i, j] = line[j];
                }
            }

            var wateredScan = new Day17().Pour(scan);
            var wateredMap = Day17.Print(wateredScan);
            var (count, _) = Day17.CountWater(wateredScan, 3);

            Assert.Equal(expected.Trim(), wateredMap.Trim());
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input17.txt");
            var (scan, minY) = Day17.Parse(input.Trim());
            var result = new Day17().Pour(scan);
            var (count1, count2) = Day17.CountWater(result, minY);
            _output.WriteLine(count1.ToString());
            _output.WriteLine(count2.ToString());
        }
    }
}
