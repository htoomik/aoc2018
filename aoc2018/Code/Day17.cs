using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    public class Day17
    {
        private readonly HashSet<(int, int)> _done = new HashSet<(int, int)>();

        public char[,] Pour(char[,] scan)
        {
            var originX = 0;
            for (var i = 0; i < scan.GetLength(1); i++)
            {
                if (scan[0, i] == '+')
                {
                    originX = i;
                    break;
                }
            }

            var state = Print(scan);
            var j = 0;
            while(true)
            {
                Drop(scan, 0, originX);
                j++;
                var newState = Print(scan);

                if (j % 10 == 0 || j > 110)
                {
                    File.WriteAllText($"C:\\Code\\aoc2018\\output17_{j}.txt",state);
                }

                if (newState == state)
                    break;
                state = newState;
            }

            return scan;
        }

        public static (int, int) CountWater(char[,] scan, int minY)
        {
            var count1 = 0;
            var count2 = 0;
            for (var k = minY; k < scan.GetLength(0); k++)
            {
                for (var m = 0; m < scan.GetLength(1); m++)
                {
                    if (scan[k, m] == '~')
                    {
                        count1++;
                        count2++;
                    }
                    if (scan[k, m] == '|')
                    {
                        count1++;
                    }
                }
            }
            return (count1, count2);
        }

        private void Drop(char[,] scan, int y, int x)
        {
            if (_done.Contains((x, y)))
            {
                return;
            }

            var originalY = y;

            // Down as far as possible
            while (y < scan.GetLength(0) - 1 &&
                   (scan[y + 1, x] == '|' || scan[y + 1, x] == '.'))
            {
                scan[y + 1, x] = '|';
                y++;
            }

            if (y == originalY)
            {
                if (!_done.Contains((x, originalY)))
                {
                    _done.Add((x, originalY));
                }

                return;
            }

            if (y == scan.GetLength(0) - 1)
            {
                if (!_done.Contains((x, originalY)))
                {
                    _done.Add((x, originalY));
                }

                return;
            }

            int leftx;
            int rightx;
            while (true)
            {
                // Test left
                leftx = x;
                while (leftx - 1 >= 0 &&
                       (scan[y, leftx - 1] == '.' || scan[y, leftx - 1] == '|') &&
                       (scan[y + 1, leftx] == '#' || scan[y + 1, leftx] == '~'))
                {
                    scan[y, leftx - 1] = '|';
                    leftx--;
                }

                // Test right
                rightx = x;
                while (rightx + 1 < scan.GetLength(1) &&
                       (scan[y, rightx + 1] == '.' || scan[y, rightx + 1] == '|') &&
                       (scan[y + 1, rightx] == '#' || scan[y + 1, rightx] == '~'))
                {
                    scan[y, rightx + 1] = '|';
                    rightx++;
                }

                // If there is support, fill. Else, drop down and spread in both directions
                if ((scan[y + 1, leftx] == '#' || scan[y + 1, leftx] == '~') &&
                    (scan[y + 1, rightx] == '#' || scan[y + 1, rightx] == '~') &&
                    scan[y, leftx - 1] == '#' &&
                    scan[y, rightx + 1] == '#')
                {
                    for (var i = leftx; i <= rightx; i++)
                    {
                        scan[y, i] = '~';
                    }

                    // Up one step and try spread again
                    y--;
                    if (y == originalY)
                        break;
                }
                else
                {
                    break;
                }
            }

            var stateBefore = Print(scan);

            Drop(scan, y, leftx);
            Drop(scan, y, rightx);

            var stateAfter = Print(scan);

            if (stateBefore == stateAfter)
            {
                _done.Add((x, originalY));
            }
        }
        
        public static (char[,], int) Parse(string input)
        {
            var lines = input.Split("\n").Select(s => s.Trim());
            var veins = lines.Select(ParseLine).ToList();
            var minX = veins.Min(vein => vein.MinX);
            var maxX = veins.Max(vein => vein.MaxX);
            var minY = veins.Min(vein => vein.MinY);
            var maxY = veins.Max(vein => vein.MaxY);

            // Y: from the surface to the bottom -> no scaling
            // X: from (min - 1) to (max + 1) -> subtract minX - 1
            // [y, x]
            var array = new char[maxY + 1, maxX - minX + 3];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = '.';
                }
            }

            foreach (var vein in veins)
            {
                if (vein.X.HasValue)
                {
                    for (var y = vein.YRange.Item1.Value; y <= vein.YRange.Item2.Value; y++)
                    {
                        array[y, vein.X.Value - (minX - 1)] = '#';
                    }
                }
                else
                {
                    for (var x = vein.XRange.Item1.Value; x <= vein.XRange.Item2.Value; x++)
                    {
                        array[vein.Y.Value, x - (minX - 1)] = '#';
                    }
                }
            }

            array[0, 500 - (minX - 1)] = '+';

            return (array, minY);
        }

        private static Vein ParseLine(string line)
        {
            var parts = line.Split(", ");
            var yPart = parts[0].Contains("y") ? parts[0] : parts[1];
            var xPart = parts[0].Contains("x") ? parts[0] : parts[1];
            if (yPart.Contains(".."))
            {
                var yRange = yPart.Split("=")[1].Split("..");
                return new Vein
                {
                    X = int.Parse( xPart.Split("=")[1]),
                    YRange = (int.Parse(yRange[0]), int.Parse(yRange[1]))
                };
            }
            if (xPart.Contains(".."))
            {
                var xRange = xPart.Split("=")[1].Split("..");
                return new Vein
                {
                    Y= int.Parse( yPart.Split("=")[1]),
                    XRange = (int.Parse(xRange[0]), int.Parse(xRange[1]))
                };
            }
            throw new Exception("Parse error");
        }

        public static string Print(Array scan)
        {
            var sb = new StringBuilder();

            for (var y = 0; y < scan.GetLength(0); y++)
            {
                for (var x = 0; x < scan.GetLength(1); x++)
                {
                    var c = (char)scan.GetValue(y, x);
                    sb.Append(c == 0 ? '.' : c);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private class Vein
        {
            public int? X { get; set; }
            public int? Y { get; set; }
            public (int?, int?) XRange { get; set; }
            public (int?, int?) YRange { get; set; }

            public int MinX => Math.Min(ValueOrMax(X), ValueOrMax(XRange.Item1));
            public int MaxX => Math.Min(ValueOrMin(X), ValueOrMax(XRange.Item1));
            public int MinY => Math.Min(ValueOrMax(Y), ValueOrMax(YRange.Item1));
            public int MaxY => Math.Min(ValueOrMin(Y), ValueOrMax(YRange.Item1));

            private static int ValueOrMax(int? i)
            {
                return i ?? int.MaxValue;
            }

            private static int ValueOrMin(int? i)
            {
                return i ?? int.MinValue;
            }
        }
    }
}
