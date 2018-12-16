using System.Linq;
using aoc2018.Code.Day15;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test.Test15
{
    public class GameTests
    {
        private readonly ITestOutputHelper _output;

        public GameTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SampleGame_1()
        {
            const string map = @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

            var engine = Helper.CreateEngine(map);

            const string expected = @"
#######
#..G..#
#...EG#
#.#G#G#
#...#E#
#.....#
#######";
            var expectedGoblinHps = new[] {200, 197, 200, 197};
            var expectedElfHps = new[] { 197, 197 };

            engine.RunGame(1);

            var finalMap = engine.Print();
            Assert.Equal(expected.Trim(), finalMap.Trim());

            var goblinHps = engine.Units.Where(u => u.Race == Race.Goblin).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedGoblinHps, goblinHps);

            var elfHps = engine.Units.Where(u => u.Race == Race.Elf).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedElfHps, elfHps);
        }

        [Fact]
        public void SampleGame_2()
        {
            const string map = @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

            var engine = Helper.CreateEngine(map);

            const string expected = @"
#######
#...G.#
#..GEG#
#.#.#G#
#...#E#
#.....#
#######";
            var expectedGoblinHps = new[] {200, 200, 194, 194};
            var expectedElfHps = new[] { 188, 194 };

            engine.RunGame(2);

            var finalMap = engine.Print();
            Assert.Equal(expected.Trim(), finalMap.Trim());

            var goblinHps = engine.Units.Where(u => u.Race == Race.Goblin).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedGoblinHps, goblinHps);

            var elfHps = engine.Units.Where(u => u.Race == Race.Elf).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedElfHps, elfHps);
        }

        [Fact]
        public void SampleGame_23()
        {
            const string map = @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

            var engine = Helper.CreateEngine(map);

            const string expected = @"
#######
#...G.#
#..G.G#
#.#.#G#
#...#E#
#.....#
#######";
            var expectedGoblinHps = new[] {200, 200, 131, 131};
            var expectedElfHps = new[] { 131 };

            engine.RunGame(23);

            var finalMap = engine.Print();
            Assert.Equal(expected.Trim(), finalMap.Trim());

            var goblinHps = engine.Units.Where(u => u.Race == Race.Goblin).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedGoblinHps, goblinHps);

            var elfHps = engine.Units.Where(u => u.Race == Race.Elf).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedElfHps, elfHps);
        }

        [Fact]
        public void SampleGame_47()
        {
            const string map = @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

            var engine = Helper.CreateEngine(map);

            const string expectedState = @"
#######
#G....#
#.G...#
#.#.#G#
#...#.#
#....G#
#######";
            var expectedGoblinHps = new[] {200, 131, 59, 200};
            var expectedElfHps = new int[] { };

            engine.RunGame(47);

            var result = engine.Print();
            Assert.Equal(expectedState.Trim(), result.Trim());

            var goblinHps = engine.Units.Where(u => u.Race == Race.Goblin).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedGoblinHps, goblinHps);

            var elfHps = engine.Units.Where(u => u.Race == Race.Elf).InReadingOrder().Select(u => u.HitPoints).ToArray();
            Assert.Equal(expectedElfHps, elfHps);
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_47()
        {
            const string map = @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            Assert.Equal(47, engine.Rounds);
            Assert.Equal(590, engine.Units.Sum(u => u.HitPoints));
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_37()
        {
            const string map = @"
#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            var finalState = engine.Print();
            var expectedState = @"
#######
#...#E#
#E#...#
#.E##.#
#E..#E#
#.....#
#######";
            _output.WriteLine(finalState);
            Assert.Equal(expectedState.Trim(), finalState.Trim());

            Assert.Equal(37, engine.Rounds);
            Assert.Equal(982, engine.Units.Sum(u => u.HitPoints));

            var remainingHitPoints = engine.Units.InReadingOrder().Select(u => u.HitPoints).ToArray();
            var expected = new[] { 200, 197, 185, 200, 200 };
            Assert.Equal(expected, remainingHitPoints);
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_46()
        {
            const string map = @"
#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            Assert.Equal(46, engine.Rounds);
            Assert.Equal(859, engine.Units.Sum(u => u.HitPoints));

            var remainingHitPoints = engine.Units.InReadingOrder().Select(u => u.HitPoints).ToArray();
            var expected = new[] { 164, 197, 200, 98, 200 };
            Assert.Equal(expected, remainingHitPoints);
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_35()
        {
            const string map = @"
#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            Assert.Equal(35, engine.Rounds);
            Assert.Equal(793, engine.Units.Sum(u => u.HitPoints));

            var remainingHitPoints = engine.Units.InReadingOrder().Select(u => u.HitPoints).ToArray();
            var expected = new[] { 200, 98, 200, 95, 200 };
            Assert.Equal(expected, remainingHitPoints);
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_54()
        {
            const string map = @"
#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            Assert.Equal(54, engine.Rounds);
            Assert.Equal(536, engine.Units.Sum(u => u.HitPoints));

            var remainingHitPoints = engine.Units.InReadingOrder().Select(u => u.HitPoints).ToArray();
            var expected = new[] { 200, 98, 38, 200 };
            Assert.Equal(expected, remainingHitPoints);
        }

        [Fact]
        public void SampleGame_ToTheBitterEnd_20()
        {
            const string map = @"
#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########";

            var engine = Helper.CreateEngine(map);

            engine.RunGame();

            Assert.Equal(20, engine.Rounds);
            Assert.Equal(937, engine.Units.Sum(u => u.HitPoints));

            var remainingHitPoints = engine.Units.InReadingOrder().Select(u => u.HitPoints).ToArray();
            var expected = new[] { 137, 200, 200, 200, 200 };
            Assert.Equal(expected, remainingHitPoints);
        }
    }
}
