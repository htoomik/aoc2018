using System;
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
           /*
            frontier = Queue()
            frontier.put(start)
            came_from = {}
            came_from[start] = None

            while not frontier.empty():
               current = frontier.get()

               if current == goal: 
                  break

               for next in graph.neighbors(current):
                  if next not in came_from:
                     frontier.put(next)
                     came_from[next] = current
            */
            
            var frontier = new Queue<Coords>();
            var cameFrom = new Dictionary<Coords, Coords?> { [startFrom] = null };

            frontier.Enqueue(startFrom);
            while (frontier.Count != 0)
            {
                var current = frontier.Dequeue();
                if (current.Equals(target))
                {
                    break;
                }

                var neighbours = GetFreeNeighbours(current, walls, units);
                foreach (var next in neighbours)
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }

            if (!cameFrom.ContainsKey(target))
            {
                return null;
            }

            var length = 0;
            Coords? backtracking = target;
            while (backtracking != null)
            {
                length++;
                backtracking = cameFrom[backtracking.Value];
            }

            return new Route
            {
                Target = target,
                Length = length - 1
            };
        }
    }
}
