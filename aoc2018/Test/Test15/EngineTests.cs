using System.Linq;
using aoc2018.Code.Day15;
using Xunit;

namespace aoc2018.Test.Test15
{
    public class EngineTests
    {
        [Fact]
        public void GetAdjacentEnemies()
        {
            const string map = @"
G....
..G..
.EEG.
..G..
...G.";
            var engine = Helper.CreateEngine(map);
            var attackingElf = Helper.TopLeftElf(engine);

            var enemies = engine.GetAdjacentEnemies(attackingElf);
            Assert.Equal(3, enemies.Count);
            Assert.True(enemies.All(e => e.Race == Race.Goblin));

            Assert.Contains(enemies, e => e.Row == 1 && e.Col == 2);
            Assert.Contains(enemies, e => e.Row == 2 && e.Col == 3);
            Assert.Contains(enemies, e => e.Row == 3 && e.Col == 2);
        }

        [Fact]
        public void GetAllEnemies()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var enemies = engine.GetAllEnemies(elf);
            Assert.Equal(3, enemies.Count);
        }
        
        [Fact]
        public void GetSquaresInRangeOf()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var enemies = engine.GetAllEnemies(elf);
            var squares = engine.GetSquaresInRangeOf(enemies).ToList();

            /*
             Expected:
                #######
                #E.?G?#
                #.?.#?#
                #?G?#G#
                #######
            */

            Assert.Equal(6, squares.Count);
            Assert.Contains(squares, c => c.Row == 1 && c.Col == 3);
        }

        [Fact]
        public void GetSquaresInRangeOf_DoesNotIncludeOccupiedSquares()
        {
            const string map = @"
#######
#E....#
#.G.#.#
#.G.#.#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var enemies = engine.GetAllEnemies(elf);
            var squares = engine.GetSquaresInRangeOf(enemies).ToList();

            /*
             Expected:
                #######
                #E?...#
                #?G?#.#
                #?G?#.#
                #######
            */

            Assert.Equal(5, squares.Count);
            Assert.DoesNotContain(squares, c => c.Row == 2 && c.Col == 2);
            Assert.DoesNotContain(squares, c => c.Row == 3 && c.Col == 2);
        }

        [Fact]
        public void GetReachableTargets()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var enemies = engine.GetAllEnemies(elf);
            var targets = engine.GetSquaresInRangeOf(enemies).ToList();
            var reachableTargets = engine.GetReachableTargets(elf, targets);

            /*
             Expected:
                #######
                #E.@G.#
                #.@.#.#
                #@G@#G#
                #######
             */

            Assert.Equal(4, reachableTargets.Count);
            Assert.Contains(reachableTargets, t => t.Row == 1 && t.Col == 3);
            Assert.Contains(reachableTargets, t => t.Row == 2 && t.Col == 2);
            Assert.Contains(reachableTargets, t => t.Row == 3 && t.Col == 1);
            Assert.Contains(reachableTargets, t => t.Row == 3 && t.Col == 3);
        }

        [Fact]
        public void FindShortestRoute()
        {
            const string map = @"
#######
#.E...#
#.....#
#...G.#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = new Unit(1, 2, Race.Elf);
            var target = new Coords(3, 5); // Bottom right corner
            var shortestRoute = engine.FindShortestRoute(elf, target);

            /*
             Expected:
                #######
                #.E--\#
                #....|#
                #...G|#
                #######
             */

            Assert.Equal(5, shortestRoute.Length);
            Assert.Equal(target, shortestRoute.Target);
        }
    }
}
