using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2018.Code.Day15
{
    public class Engine
    {
        public const int DefaultAttackPower = 3;

        private bool[,] _walls;
        public List<Unit> Units { get; private set; }
        public int Rounds;

        public void Initialize(List<string> input, int elfPower = DefaultAttackPower)
        {
            (_walls, Units) = Parser.Parse(input, this);
            foreach (var unit in Units)
            {
                unit.AttackPower = unit.Race == Race.Elf ? elfPower : DefaultAttackPower;
            }
        }

        public void RunGame(int? rounds = null)
        {
            while (true)
            {
                var sortedUnits = Units.InReadingOrder();
                var aborted = false;
                foreach (var unit in sortedUnits)
                {
                    // Dead units are removed from the main Units list but not from sortedUnits.
                    // Therefore, skip them here.
                    if (unit.HitPoints <= 0)
                        continue;

                    var enemyCount = Units.Count(u => u.Race != unit.Race);
                    if (enemyCount == 0)
                    {
                        aborted = true;
                        break;
                    }

                    unit.Move();
                    unit.Attack();
                }

                if (aborted)
                {
                    break;
                }

                Rounds++;
                
                if (rounds.HasValue && rounds == Rounds)
                    break;
            }
        }

        public List<Unit> GetAdjacentEnemies(Unit unit)
        {
            return GetAllEnemies(unit)
                .Where(u => u.Row == unit.Row && Math.Abs(u.Col - unit.Col) == 1 ||
                            u.Col == unit.Col && Math.Abs(u.Row - unit.Row) == 1)
                .ToList();
        }

        public List<Unit> GetAllEnemies(Unit unit)
        {
            return Units.Where(u => u.Race != unit.Race).ToList();
        }

        public HashSet<Coords> GetSquaresInRangeOf(List<Unit> enemies)
        {
            var squaresInRange = new HashSet<Coords>();
            foreach (var enemy in enemies)
            {
                var west = new Coords(enemy.Row, enemy.Col - 1);
                var east = new Coords(enemy.Row, enemy.Col + 1);
                var north = new Coords(enemy.Row - 1, enemy.Col);
                var south = new Coords(enemy.Row + 1, enemy.Col);

                if (IsFree(west))
                    squaresInRange.Add(west);
                if (IsFree(east))
                    squaresInRange.Add(east);
                if (IsFree(north))
                    squaresInRange.Add(north);
                if (IsFree(south))
                    squaresInRange.Add(south);
            }

            return squaresInRange;
        }

        private bool IsFree(Coords coords)
        {
            return PathFinder.IsFree(_walls, Units, coords);
        }

        public List<Coords> GetReachableTargets(Unit unit, IEnumerable<Coords> moveTargets)
        {
            return PathFinder.GetReachableTargets(_walls, Units, unit.GetCoords(), moveTargets);
        }

        public Route FindShortestRoute(Unit unit, Coords target)
        {
            return PathFinder.FindShortestRoute(unit.GetCoords(), target, _walls, Units);
        }

        public Coords ChooseNextStepTowards(Unit unit, Coords target, int routeLength)
        {
            var freeNeighbours = PathFinder.GetFreeNeighbours(unit.GetCoords(), _walls, Units);
            var eligibleNeighbours = new List<Coords>();

            foreach (var neighbour in freeNeighbours)
            {
                var shortestRoute = PathFinder.FindShortestRoute(neighbour, target, _walls, Units);
                
                // Some first steps will not lead to the target at all
                if (shortestRoute != null && shortestRoute.Length == routeLength - 1)
                {
                    eligibleNeighbours.Add(neighbour);
                }
            }

            return eligibleNeighbours.TopLeft();
        }

        public string Print()
        {
            var maxI = _walls.GetLength(0);
            var maxJ = _walls.GetLength(1);
            var map = new char[maxI, maxJ];

            for (int i = 0; i < maxI; i++)
            {
                for (int j = 0; j < maxJ; j++)
                {

                    map[i, j] = _walls[i, j] ? '#' : '.';
                }
            }

            foreach (var unit in Units)
            {
                map[unit.Row, unit.Col] = unit.Race == Race.Elf ? 'E' : 'G';
            }
            
            var sb = new StringBuilder();
            for (int i = 0; i < maxI; i++)
            {
                for (int j = 0; j < maxJ; j++)
                {
                    sb.Append(map[i, j]);
                }

                sb.AppendLine();
            }

            var print = sb.ToString();
            return print;
        }

        public Unit ChooseAttackTarget(Unit attackingUnit)
        {
            var adjacentEnemies = GetAdjacentEnemies(attackingUnit);
            if (!adjacentEnemies.Any())
                return null;

            var lowestHitPoints = adjacentEnemies.Min(u => u.HitPoints);
            var weakestEnemies = adjacentEnemies.Where(u => u.HitPoints == lowestHitPoints);
            var topLeftEnemy = weakestEnemies.TopLeft();
            return topLeftEnemy;
        }
    }
}
