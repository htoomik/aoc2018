using System;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    class Day17
    {
        public static (Array wateredScan, int watercount) Pour(Array scan, int? drops)
        {
            if (drops.HasValue)
            {
                for (int i = 0; i < drops; i++)
                {
                    AddDrop(scan, 0, 500);
                }
                var waterCount = CountWater(scan);
                return (scan, waterCount);
            }
            else
            {
                var standingWaterCount = 0;
                var runningWaterCount = 0;
                while (true)
                {
                    AddDrop(scan, 0, 500);
                    var newRunningWaterCount = Count(scan, '|');
                    var newStandingWaterCount = Count(scan, '~');
                    if (newRunningWaterCount == runningWaterCount &&
                        newStandingWaterCount == standingWaterCount)
                        break;
                    standingWaterCount = newStandingWaterCount;
                    runningWaterCount = newRunningWaterCount;
                }

                return (scan, standingWaterCount + runningWaterCount);
            }
        }

        private static int CountWater(Array scan)
        {
            return Count(scan, '|') + Count(scan, '~');
        }

        private static int Count(Array scan, char t)
        {
            var count = 0;
            for (int y = scan.GetLowerBound(0); y <= scan.GetUpperBound(0); y++)
            {
                for (int x = scan.GetLowerBound(1); x <= scan.GetUpperBound(1); x++)
                {
                    var c = (char) scan.GetValue(y, x);
                    if (c == t)
                        count++;
                }
            }

            return count;
        }

        private static void AddDrop(Array scan, int y, int x)
        {
            if (!DownAndSpread(scan, y, x, -1))
                DownAndSpread(scan, y, x, +1);
        }

        private static bool DownAndSpread(Array scan, int y, int x, int direction)
        {
            if (y == scan.GetUpperBound(0))
                return false;

            var moved = false;

            // Move down as far as possible
            while (CanMoveDown(scan, y, x))
            {
                moved = true;
                y++;
                scan.SetValue('|', y, x);
            }

            if (!moved)
                return false;

            if (y == scan.GetUpperBound(0))
                return false;

            var movedSideways = false;
            // Spread horizontally
            if (direction == -1)
            {
                while (CanMoveLeft(scan, y, x))
                {
                    x--;
                    scan.SetValue('|', y, x);
                    movedSideways = true;
                }
            }
            else
            {
                while (CanMoveRight(scan, y, x))
                {
                    x++;
                    scan.SetValue('|', y, x);
                    movedSideways = true;
                }
            }

            if (HasSupport(scan, y, x))
            {
                var canMoveInOtherDirection = false;
                if (direction == -1)
                {
                    var behindMe = (char) scan.GetValue(y, x + 1);
                    canMoveInOtherDirection = behindMe != '#' && behindMe != '~';
                }

                if (movedSideways || !canMoveInOtherDirection)
                {
                    scan.SetValue('~', y, x);
                    return true;
                }
            }

            if (direction == -1 && x == scan.GetLowerBound(1))
                return false;

            if (direction == 1 && x == scan.GetUpperBound(1))
                return false;

            if ((char) scan.GetValue(y, x + direction) == '#')
                return false;

            if (!DownAndSpread(scan, y, x, -1))
                return DownAndSpread(scan, y, x, +1);
            return false;
        }

        private static bool CanMoveDown(Array scan, int y, int x)
        {
            if (y == scan.GetUpperBound(0))
                return false;

            var below = (char) scan.GetValue(y + 1, x);
            return below == '|' || below == 0;
        }

        private static bool CanMoveLeft(Array scan, int y, int x)
        {
            if (x == scan.GetLowerBound(1))
                return false;

            // spread until you (a) hit a wall or (b) previous water or (c) pour over an edge
            if ((char) scan.GetValue(y, x - 1) == '#')
                return false;
            
            if ((char) scan.GetValue(y, x - 1) == '~')
                return false;

            if (CanMoveDown(scan, y, x))
                return false;

            return true;
        }

        private static bool CanMoveRight(Array scan, int y, int x)
        {
            if (x == scan.GetUpperBound(1))
                return false;

            // spread until you (a) hit a wall or (b) previous water or (c) pour over an edge
            if ((char) scan.GetValue(y, x + 1) == '#')
                return false;
            
            if ((char) scan.GetValue(y, x + 1) == '~')
                return false;

            if (CanMoveDown(scan, y, x))
                return false;

            return true;
        }

        private static bool HasSupport(Array scan, int y, int x)
        {
            if (y == scan.GetUpperBound(0))
                return false;

            for (int i = x; i <= scan.GetUpperBound(1); i++)
            {
                var below = (char)scan.GetValue(y + 1, i);
                if (below != '#' && below != '~')
                    return false;

                if ((char)scan.GetValue(y, i) == '#')
                    break;
            }

            for (int i = x; i >= scan.GetLowerBound(1); i--)
            {
                var below = (char)scan.GetValue(y + 1, i);
                if (below != '#' && below != '~')
                    return false;

                if ((char)scan.GetValue(y, i) == '#')
                    break;
            }

            return true;
        }

        public static Array Parse(string input)
        {
            var lines = input.Split("\n").Select(s => s.Trim());
            var veins = lines.Select(ParseLine).ToList();
            var minX = veins.Min(vein => vein.MinX);
            var maxX = veins.Max(vein => vein.MaxX);
            var minY = veins.Min(vein => vein.MinY);
            var maxY = veins.Max(vein => vein.MaxY);

            // Y: from the surface to the bottom
            // X: from (min - 1) to (max + 1)
            var array = Array.CreateInstance(typeof(char),
                new[] { maxY + 1, maxX - minX + 3 },
                new[] { 0, minX - 1 });

            foreach (var vein in veins)
            {
                if (vein.X.HasValue)
                {
                    for (var y = vein.YRange.Item1.Value; y <= vein.YRange.Item2.Value; y++)
                    {
                        array.SetValue('#', y, vein.X.Value);
                    }
                }
                else
                {
                    for (var x = vein.XRange.Item1.Value; x <= vein.XRange.Item2.Value; x++)
                    {
                        array.SetValue('#', vein.Y.Value, x);
                    }
                }
            }

            array.SetValue('+', 0, 500);

            return array;
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

            for (var y = scan.GetLowerBound(0); y <= scan.GetUpperBound(0); y++)
            {
                for (var x = scan.GetLowerBound(1); x <= scan.GetUpperBound(1); x++)
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
