using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace aoc2018.Code
{
    class Day05
    {
        public static int Solve1(string data)
        {
            var chars = data.ToCharArray().Select(c => (short)c).ToList();
            return SolveInner(chars);
        }

        public static int Solve2(string data)
        {
            var chars = data.ToCharArray().Select(c => (short)c).ToList();
            var distinct = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

            var shortest = data.Length;
            foreach (var s in distinct)
            {
                var input = chars.Where(c => c != s && c != s - 32).ToList();
                var result = SolveInner(input);
                if (result < shortest)
                    shortest = result;
            }
            return shortest;
        }

        private static int SolveInner(List<short> chars)
        {
            bool removed = true;
            while (removed)
            {
                removed = false;
                for (var i = chars.Count - 2; i >= 0; i--)
                {
                    var c1 = chars[i];
                    var c2 = chars[i + 1];
                    if (Math.Abs(c1 - c2) == 32)
                    {
                        chars.RemoveAt(i + 1);
                        chars.RemoveAt(i);
                        i--;
                        removed = true;
                    }
                }
            }

            return chars.Count;
        }
    }
}
