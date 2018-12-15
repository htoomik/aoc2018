using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public class Engine
    {
        private bool[,] _walls;
        public List<Unit> Units { get; private set; }

        public void Initialize(List<string> input)
        {
            (_walls, Units) = Parser.Parse(input, this);
        }

        public string RunGame()
        {
            while (true)
            {
                var sortedUnits = Units.OrderBy(p => p.Row).ThenBy(p => p.Col);
                foreach (var unit in sortedUnits)
                {
                    unit.Move();
                }
                foreach (var unit in sortedUnits)
                {
                    unit.Attack();
                }
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

        public IEnumerable<Coords> GetSquaresInRangeOf(List<Unit> enemies)
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
            return PathFinder.GetReachableTargets(_walls, Units, new Coords(unit.Row, unit.Col), moveTargets);
        }

        public Route FindShortestRoute(Coords coords, Coords target)
        {
            return PathFinder.FindShortestRoute(coords, target, _walls, Units);
        }

        public List<Route> FindAllRoutes(Unit unit, Coords target, int length)
        {
            throw new NotImplementedException();
        }
    }
}
