using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc2018.Code
{
    class Day19
    {
        public static (int[] state, int iterations) Solve(string[] input, int[] initialValues = null, int? its = null, int? printFrom = null, string printTo = null)
        {
            var ipRegister = int.Parse(input[0].Replace("#ip ", ""));
            var instructions = Parse(input.Skip(1));
            var state = initialValues ?? new int[6];
            
            const int window = 10000;
            var path = $"C:\\Code\\aoc2018\\output19_{printTo}.txt";
            File.Delete(path);

            var i = 0;
            while (true)
            {
                i++;
                var ip = state[ipRegister];

                var instruction = instructions[ip];
                instruction.Action(state, instruction.A, instruction.B, instruction.C);

                if (printFrom.HasValue && i > printFrom + window)
                    break;

                if (state[ipRegister] >= instructions.Count - 1)
                    break;

                if (state[ipRegister] < 0)
                    break;

                if (i == its)
                    break;

                state[ipRegister]++;

                if (printFrom.HasValue && i > printFrom && i < printFrom + window)
                {
                    File.AppendAllText(path, string.Join(",", state) + "\r\n");
                }
            }

            return (state, i);
        }

        public static (int, int) Solve2()
        {
            var a = 1;
            var b = 2;
            const int z = 10551345;

            while (true)
            {
                if (z % b == 0)
                    a = a + b;
                b++;

                if (b > z)
                    break;
            }

            return (a, b);
        }

        private static List<Operation> Parse(IEnumerable<string> input)
        {
            return input
                .Select(s => s.Split(" "))
                .Select(parts => new Operation
                {
                    Name = parts[0],
                    Action = Operations[parts[0]],
                    A = int.Parse(parts[1]),
                    B = int.Parse(parts[2]),
                    C = int.Parse(parts[3]),
                })
                .ToList();
        }

        #region OperationsSet
        private static readonly Dictionary<string, Action<int[], int,int,int>> Operations = new Dictionary<string, Action<int[], int,int,int>>
        {
            { "addr", Addr },
            { "addi", Addi },
            { "mulr", Mulr },
            { "muli", Muli },
            { "banr", Banr },
            { "bani", Bani },
            { "borr", Borr },
            { "bori", Bori },
            { "setr", Setr },
            { "seti", Seti },
            { "gtri", Gtri },
            { "gtir", Gtir },
            { "gtrr", Gtrr },
            { "eqir", Eqir },
            { "eqri", Eqri },
            { "eqrr", Eqrr }
        };
        #endregion

        #region Operations

        private static void Addr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + registers[b];
        }

        private static void Addi(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] + b;
        }

        private static void Mulr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * registers[b];
        }

        private static void Muli(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] * b;
        }

        private static void Banr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & registers[b];
        }

        private static void Bani(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] & b;
        }

        private static void Borr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | registers[b];
        }

        private static void Bori(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] | b;
        }

        private static void Setr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a];
        }

        private static void Seti(int[] registers, int a, int b, int c)
        {
            registers[c] = a;
        }

        private static void Gtir(int[] registers, int a, int b, int c)
        {
            registers[c] = a > registers[b] ? 1 : 0;
        }

        private static void Gtri(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > b ? 1 : 0;
        }

        private static void Gtrr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] > registers[b] ? 1 : 0;
        }

        private static void Eqir(int[] registers, int a, int b, int c)
        {
            registers[c] = a == registers[b] ? 1 : 0;
        }

        private static void Eqri(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == b ? 1 : 0;
        }

        private static void Eqrr(int[] registers, int a, int b, int c)
        {
            registers[c] = registers[a] == registers[b] ? 1 : 0;
        }

        #endregion

        private class Operation
        {
            public string Name { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
            public Action<int[], int, int, int> Action { get; set; }
        }
    }
}
