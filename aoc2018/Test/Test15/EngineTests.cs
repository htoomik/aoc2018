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
            var attackingElf = Helper.TopLeftElf(engine, 1);

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

        [Fact]
        public void Print()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var printed = engine.Print();
            Assert.Equal(map.Trim(), printed.Trim());
        }
        
        [Fact]
        public void Movements_3()
        {
            const string map = @"
#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########";
            var engine = Helper.CreateEngine(map);
            engine.RunGame(3);

            var result = engine.Print();
            const string expected = @"
#########
#.......#
#..GGG..#
#..GEG..#
#G..G...#
#......G#
#.......#
#.......#
#########";
            Assert.Equal(expected.Trim(), result.Trim());
        }

        [Fact]
        public void ChooseAttackTarget()
        {
            const string map = @"
G....
..G..
..EG.
..G..
...G.";
            var engine = Helper.CreateEngine(map);
            var goblins = engine.Units.Where(u => u.Race == Race.Goblin).ToList();
            goblins.Single(u => u.Row == 0).HitPoints = 9;
            goblins.Single(u => u.Row == 1).HitPoints = 4;
            goblins.Single(u => u.Row == 2).HitPoints = 2;
            goblins.Single(u => u.Row == 3).HitPoints = 2;
            goblins.Single(u => u.Row == 4).HitPoints = 1;

            var elf = engine.Units.Single(u => u.Race == Race.Elf);

            var attackTarget = engine.ChooseAttackTarget(elf);
            Assert.Equal(2, attackTarget.Row);
        }
    }
}
