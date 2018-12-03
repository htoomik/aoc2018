using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day03
    {
        public static int Solve1(List<string> data)
        {
            var claims = data.Select(Parse).ToList();

            var fabric = new Dictionary<SquareInch, int>();
            foreach (var claim in claims)
            {
                for (var i = 0; i < claim.Width; i++)
                {
                    for (var j = 0; j < claim.Height; j++)
                    {
                        var claimedSquare = new SquareInch { Left = claim.Left + i, Top = claim.Top + j };
                        if (!fabric.ContainsKey(claimedSquare))
                        {
                            fabric.Add(claimedSquare, 0);
                        }

                        fabric[claimedSquare]++;
                    }
                }
            }

            return fabric.Values.Count(v => v > 1);
        }

        public static int Solve2(List<string> data)
        {
            var claims = data.Select(Parse).ToList();

            var fabric = new Dictionary<SquareInch, List<int>>();
            var noOverlaps = new HashSet<int>();

            foreach (var claim in claims)
            {
                var hasOverlaps = false;
                for (var i = 0; i < claim.Width; i++)
                {
                    for (var j = 0; j < claim.Height; j++)
                    {
                        var claimedSquare = new SquareInch { Left = claim.Left + i, Top = claim.Top + j };
                        if (!fabric.ContainsKey(claimedSquare))
                        {
                            fabric.Add(claimedSquare, new List<int>());
                        }
                        else
                        {
                            hasOverlaps = true;
                            foreach (var overlapping in fabric[claimedSquare])
                            {
                                if (noOverlaps.Contains(overlapping))
                                {
                                    noOverlaps.Remove(overlapping);
                                }
                            }
                        }

                        fabric[claimedSquare].Add(claim.Id);
                    }
                }

                if (!hasOverlaps)
                {
                    noOverlaps.Add(claim.Id);
                }
            }

            return noOverlaps.Single();
        }

        private static Claim Parse(string line)
        {
            var s1 = line.Split(" @ ");
            var s2 = s1[1].Split(": ");
            var s3 = s2[0].Split(",");
            var s4 = s2[1].Split("x");

            return new Claim
            {
                Id = int.Parse(s1[0].Replace("#", "")),
                Left =  int.Parse(s3[0]),
                Top = int.Parse(s3[1]),
                Width = int.Parse(s4[0]),
                Height = int.Parse(s4[1])
            };
        }

        private struct Claim
        {
            public int Id;
            public int Top;
            public int Left;
            public int Width;
            public int Height;
        }

        private struct SquareInch
        {
            public int Top;
            public int Left;

            public bool Equals(SquareInch other)
            {
                return Top == other.Top && Left == other.Left;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is SquareInch && Equals((SquareInch) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Top * 397) ^ Left;
                }
            }
        }
    }
}
