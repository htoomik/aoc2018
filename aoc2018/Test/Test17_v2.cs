using System;
using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test17_v2
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

        private const string Map_2 = @"
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
        
        private const string Expected_2 = @"
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

        private const int Count_2 = 4 * 9 + 7 + 5 + 2 * 7;

        private const string Map_3 = @"
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
        
        private const string Expected_3 = @"
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

        private const int Count_3 = 62;

        #endregion

        public Test17_v2(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Parse()
        {
            
            var (scan, _) = Day17_v2.Parse(Input.Trim());
            var map = Day17_v2.Print(scan);
            Assert.Equal(Map.Trim(), map.Trim());
        }

        [Fact]
        public void Test()
        {
            var (scan, minY) = Day17_v2.Parse(Input.Trim());
            var (wateredScan, waterCount) = new Day17_v2().Pour(scan, minY);
            var wateredMap = Day17_v2.Print(wateredScan);
            Assert.Equal(Expected.Trim(), wateredMap.Trim());
            Assert.Equal(Count, waterCount);
        }

        [Theory]
        [InlineData(Map_2, Expected_2, Count_2)]
        [InlineData(Map_3, Expected_3, Count_3)]
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

            var (wateredScan, count) = new Day17_v2().Pour(scan, 3);
            var wateredMap = Day17_v2.Print(wateredScan);
            File.WriteAllText("C:\\Code\\aoc2018\\output17_testx.txt", wateredMap);

            Assert.Equal(expected.Trim(), wateredMap.Trim());
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input17.txt");
            var (scan, minY) = Day17_v2.Parse(input.Trim());
            var result = new Day17_v2().Pour(scan, minY);
            File.WriteAllText("C:\\Code\\aoc2018\\output17.txt", Day17_v2.Print(result.wateredScan));
            _output.WriteLine(result.watercount.ToString());
        }
    }
}
