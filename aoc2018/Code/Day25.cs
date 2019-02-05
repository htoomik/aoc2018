using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    public static class Day25
    {
        public static int Solve(string input)
        {
            var points = Parse(input);

            var constellations = new List<List<Point>>();

            // First attempt
            foreach (var point in points)
            {
                var connected = false;
                foreach (var constellation in constellations)
                {
                    if (Connected(point, constellation))
                    {
                        constellation.Add(point);
                        connected = true;
                        break;
                    }
                }

                if (!connected)
                {
                    constellations.Add(new List<Point> { point });
                }
            }

            // Try and join up existing constellations
            var changed = true;
            while (changed)
            {
                changed = false;

                for (int i = 0; i < constellations.Count; i++)
                {
                    for (int j = 0; j < constellations.Count; j++)
                    {
                        if (i == j)
                            continue;

                        foreach (var point1 in constellations[i])
                        {
                            if (Connected(point1, constellations[j]))
                            {
                                constellations[i].AddRange(constellations[j]);
                                constellations.RemoveAt(j);
                                changed = true;
                                break;
                            }
                        }

                        if (changed)
                            break;
                    }

                    if (changed)
                        break;
                }
            }

            return constellations.Count;
        }

        private static List<Point> Parse(string input)
        {
            var lines = input.Trim().Split("\n");
            return lines
                .Select(line => line.Trim().Split(","))
                .Select(parts => new Point
                {
                    X = parts[0].ToInt(),
                    Y = parts[1].ToInt(),
                    Z = parts[2].ToInt(),
                    W = parts[3].ToInt(),
                })
                .ToList();
        }

        private static int ToInt(this string s)
        {
            return int.Parse(s.Trim());
        }

        private static bool Connected(Point point, List<Point> constellation)
        {
            return constellation.Any(p => Distance(p, point) <= 3);
        }

        private static int Distance(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) +
                   Math.Abs(point1.Y - point2.Y) +
                   Math.Abs(point1.Z - point2.Z) +
                   Math.Abs(point1.W - point2.W);
        }

        private struct Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int W { get; set; }
        }
    }
}
