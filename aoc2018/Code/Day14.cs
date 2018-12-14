using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace aoc2018.Code
{
    class Day14
    {
        public static string Solve1(int input)
        {
            var list = new List<int> { 3, 7 };
            var a = 0;
            var b = 1;
            while (list.Count < input + 10) 
            {
                Add(list, a, b);
                a = Move(list, a);
                b = Move(list, b);
            }

            var result = "";
            for (int i = 0; i < 10; i++)
            {
                result += list[input + i].ToString();
            }

            return result;
        }

        public static (string, int) Solve2(string input)
        {
            var list = new List<int> { 3, 7 };
            var target = input.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();

            var a = 0;
            var b = 1;
            int offset;
            while (!Done(list, target, out offset))
            {
                Add(list, a, b);
                a = Move(list, a);
                b = Move(list, b);
            }

            var result = list.Count - target.Count + offset;
            var output = string.Join("", list);
            return (output, result);
        }

        private static bool Done(List<int> list, List<int> target, out int offset)
        {
            offset = 0;
            if (list.Count < target.Count + 1)
                return false;

            var match1 = true;
            var match2 = true;
            for (var i = 0; i < target.Count; i++)
            {
                if (list[list.Count - 1 - i] != target[target.Count - 1 - i])
                    match1 = false;
                if (list[list.Count - 2 - i ] != target[target.Count - 1 - i])
                    match2 = false;
            }

            var done = match1 || match2;
            if (match2)
            {
                offset = -1;
            }

            return done;
        }

        private static int Move(List<int> list, int i)
        {
            var score = list[i];
            var newPos = i + 1 + score;
            if (newPos > list.Count - 1)
            {
                newPos = newPos % list.Count;
            }

            return newPos;
        }

        private static void Add(List<int> list, int a, int b)
        {
            var sum = list[a] + list[b];
            if (sum >= 10)
                list.Add(1);
            list.Add(sum % 10);
        }
    }
}
