using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    static class Day16
    {
        private static HashSet<Action<int[], int, int, int>> Operations = new HashSet<Action<int[], int, int, int>>
        {
            Addr, Addi, Mulr, Muli, Banr, Bani, Borr, Bori, 
            Setr, Seti, Gtri, Gtir, Gtrr, Eqir, Eqri, Eqrr
        };

        public static int Solve1(string input)
        {
            var partOneInput = input.Split("\n\n\n\n")[0];
            var samples = partOneInput.Split("\n\n");
            var total = 0;

            foreach (var sample in samples)
            {
                var matchingOperations = CountMatchingOperations(sample);
                if (matchingOperations >= 3)
                    total++;
            }

            return total;
        }

        public static int CountMatchingOperations(string input)
        {
            var (startingRegisters, arguments, endingRegisters) = Parse(input);
            var matches = 0;

            foreach (var operation in Operations)
            {
                var isMatch = IsMatch(operation, startingRegisters, arguments, endingRegisters);

                if (isMatch)
                {
                    matches++;
                }
            }

            return matches;
        }

        public static (int[] startingRegisters, int[] arguments, int[] endingRegisters) Parse(string input)
        {
            var lines = input.Trim().Split("\n");
            var startingRegisters = ParseRegisters(lines[0]);
            var endingRegisters = ParseRegisters(lines[2]);
            var arguments = lines[1].Trim().ToInts(" ");
            return (startingRegisters, arguments, endingRegisters);
        }

        public static bool IsMatch(Action<int[], int, int, int> operation, int[] startingRegisters, int[] arguments, int[] endingRegisters)
        {
            var newRegisters = Copy(startingRegisters);
            try
            {
                operation(newRegisters, arguments[1], arguments[2], arguments[3]);
            }
            catch
            {
                return false;
            }

            if (Equal(newRegisters, endingRegisters))
            {
                return true;
            }

            return false;
        }

        public static int[] ParseRegisters(string s)
        {
            return s
                .Replace("Before: [", "")
                .Replace("After:  [", "")
                .Replace("]", "")
                .Trim()
                .ToInts(", ");
        }

        public static int[] ToInts(this string s, string splitBy)
        {
            return s.Split(splitBy).Select(int.Parse).ToArray();
        }

        public static void Addr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + registers[b];
        }

        public static void Addi(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + b;
        }

        public static void Mulr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * registers[b];
        }

        public static void Muli(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * b;
        }

        public static void Banr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & registers[b];
        }

        public static void Bani(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & b;
        }

        public static void Borr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | registers[b];
        }

        public static void Bori(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | b;
        }

        public static void Setr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a];
        }

        public static void Seti(int[] registers, int a, int b, int c)
        {
            registers[c] = a;
        }

        public static void Gtir(int[] registers, int a, int b, int c)
        {
            registers[c] = a > registers[b] ? 1 : 0;
        }

        public static void Gtri(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > b ? 1 : 0;
        }

        public static void Gtrr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > registers[b] ? 1 : 0;
        }

        public static void Eqir(int[] registers, int a, int b, int c)
        {
            registers[c] = a == registers[b] ? 1 : 0;
        }

        public static void Eqri(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == b ? 1 : 0;
        }

        public static void Eqrr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == registers[b] ? 1 : 0;
        }

        private static int[] Copy(int[] registers)
        {
            return registers.Select(i => i).ToArray();
        }

        private static bool Equal(int[] one, int[] two)
        {
            if (one.Length != two.Length)
                return false;

            return !one.Where((value, i) => value != two[i]).Any();
        }
    }
}
