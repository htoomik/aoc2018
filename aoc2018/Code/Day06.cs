using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day06
    {
        public static int Solve1(List<string> data)
        {
            var coords = Parse(data);

            var maxX = coords.Max(c => c.X);
            var maxY = coords.Max(c => c.Y);
            var grid = new char[maxX + 1, maxY + 1];
            var counts = coords.ToDictionary(c => c.Id, c => 0);

            foreach (var coord in coords)
            {
                grid[coord.X, coord.Y] = coord.Id;
            }

            for (var x = 0; x < maxX + 1; x++)
            {
                for (var y = 0; y < maxY + 1; y++)
                {
                    var distances = new Dictionary<char, int>();
                    foreach (var coord in coords)
                    {
                        distances[coord.Id] = Manhattan(x, y, coord);
                    }

                    var shortest = distances.Values.Min();
                    var closestCoords = distances.Where(kvp => kvp.Value == shortest).ToList();
                    if (closestCoords.Count == 1)
                    {
                        var closestCoordId = closestCoords.Single().Key;
                        grid[x, y] = closestCoordId;
                        counts[closestCoordId]++;
                    }
                    else
                    {
                        grid[x, y] = '.';
                    }
                }
            }

            var infinites = GetInfinites(grid, maxX, maxY);
            if (infinites.Count == 0)
            {
                throw new Exception("Grid has no edges");
            }

            var eligible = counts.Where(kvp => !infinites.Contains(kvp.Key)).Where(kvp => kvp.Key != ' ').ToList();
            var largest = eligible.Select(kvp => kvp.Value).Max();
            var idOfLargest = eligible.Single(kvp => kvp.Value == largest).Key;

            return counts[idOfLargest];
        }

        public static int Solve2(List<string> data, int cutoff)
        {
            var coords = Parse(data);

            var maxX = coords.Max(c => c.X);
            var maxY = coords.Max(c => c.Y);
            var grid = new char[maxX + 1, maxY + 1];

            foreach (var coord in coords)
            {
                grid[coord.X, coord.Y] = coord.Id;
            }

            var count = 0;
            for (var x = 0; x < maxX + 1; x++)
            {
                for (var y = 0; y < maxY + 1; y++)
                {
                    var distances = new Dictionary<char, int>();
                    foreach (var coord in coords)
                    {
                        distances[coord.Id] = Manhattan(x, y, coord);
                    }

                    var total = distances.Values.Sum();
                    if (total < cutoff)
                    {
                        grid[x, y] = '#';
                        count++;
                    }
                    else
                    {
                        grid[x, y] = '.';
                    }
                }
            }

            return count;
        }

        private static HashSet<char> GetInfinites(char[,] grid, int maxX, int maxY)
        {
            var result = new HashSet<char>();
            for (var x = 0; x < maxX; x++)
            {
                result.Add(grid[x, 0]);
                result.Add(grid[x, maxY]);
            }
            for (var y = 0; y < maxY; y++)
            {
                result.Add(grid[0, y]);
                result.Add(grid[maxX, y]);
            }

            return result;
        }

        private static int Manhattan(int x, int y, Coord coord)
        {
            return Math.Abs(x - coord.X) + Math.Abs(y - coord.Y);
        }

        private static List<Coord> Parse(List<string> data)
        {
            var results = new List<Coord>();
            const string ids = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var i = 0;
            foreach (var line in data)
            {
                var coord = Parse(line);
                coord.Id = ids[i++];
                results.Add(coord);
            }

            return results;
        }

        private static Coord Parse(string line)
        {
            var s = line.Split(",");
            return new Coord
            {
                X = int.Parse(s[0]),
                Y = int.Parse(s[1])
            };
        }

        private struct Coord
        {
            public char Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
