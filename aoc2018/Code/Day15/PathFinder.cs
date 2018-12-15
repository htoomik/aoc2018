using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code.Day15
{
    public class PathFinder
    {
        public static bool IsFree(bool[,] walls, List<Unit> units, Coords coords)
        {
            if (walls[coords.Row, coords.Col]) 
                return false;
            if (units.Any(u => u.Row == coords.Row && u.Col == coords.Col))
                return false;
            return true;
        }

        public static List<Coords> GetReachableTargets(bool[,] walls, List<Unit> units, Coords startFrom, IEnumerable<Coords> moveTargets)
        {
            var frontier = new Queue<Coords>();
            var visited = new List<Coords>();

            frontier.Enqueue(startFrom);

            while (frontier.Any())
            {
                var next = frontier.Dequeue();
                Expand(next, frontier, visited, walls, units);
            }

            return moveTargets.Intersect(visited).ToList();
        }

        private static void Expand(Coords coords, Queue<Coords> frontier, List<Coords> visited, bool[,] walls, List<Unit> units)
        {
            var freeNeighbours = GetFreeNeighbours(coords, walls, units);

            foreach (var neighbour in freeNeighbours)
            {
                if (!visited.Contains(neighbour))
                {
                    frontier.Enqueue(neighbour);
                    visited.Add(neighbour);
                }
            }
        }

        public static IEnumerable<Coords> GetFreeNeighbours(Coords coords, bool[,] walls, List<Unit> units)
        {
            return GetNeighbours(coords)
                .Where(neighbour => WithinBounds(walls, neighbour) && 
                                    IsFree(walls, units, neighbour))
                .ToList();
        }

        private static IEnumerable<Coords> GetNeighbours(Coords coords)
        {
            return new List<Coords>
            {
                new Coords(coords.Row + 1, coords.Col),
                new Coords(coords.Row - 1, coords.Col),
                new Coords(coords.Row, coords.Col + 1),
                new Coords(coords.Row, coords.Col - 1)
            };
        }

        private static bool WithinBounds(bool[,] walls, Coords coords)
        {
            return coords.Row >= 0 &&
                   coords.Col >= 0 &&
                   coords.Row < walls.GetLength(0) &&
                   coords.Col < walls.GetLength(1);
        }

        public static Route FindShortestRoute(Coords startFrom, Coords target, bool[,] walls, List<Unit> units)
        {
            var frontier = new PriorityQueue<Coords>();
            var costSoFar = new Dictionary<Coords, int> { [startFrom] = 0 };

            frontier.Enqueue(startFrom, 0);
            while (!frontier.IsEmpty())
            {
                var current = frontier.Dequeue();
                if (current.Equals(target))
                {
                    break;
                }

                var neighbours = GetFreeNeighbours(current, walls, units);
                foreach (var next in neighbours)
                {
                    var newCost = costSoFar[current] + 1;
                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        var priority = newCost + (target.Row - next.Row) + (target.Col - next.Col);
                        frontier.Enqueue(next, priority);
                    }
                }
            }

            return new Route
            {
                Target = target,
                Length = costSoFar[target]
            };
        }

        public static List<Route> FindAllRoutes(Coords startFrom, Coords target, int length)
        {
            throw new System.NotImplementedException();
        }
    }
}
