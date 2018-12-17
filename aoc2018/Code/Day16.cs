using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    static class Day16
    {
        #region OperationsSet
        private static readonly HashSet<Operation> Operations = new HashSet<Operation>
        {
            new Operation { Action = Addr, Name = "Addr" },
            new Operation { Action = Addi, Name = "Addi" },
            new Operation { Action = Mulr, Name = "Mulr" },
            new Operation { Action = Muli, Name = "Muli" },
            new Operation { Action = Banr, Name = "Banr" },
            new Operation { Action = Bani, Name = "Bani" },
            new Operation { Action = Borr, Name = "Borr" },
            new Operation { Action = Bori, Name = "Bori" },
            new Operation { Action = Setr, Name = "Setr" },
            new Operation { Action = Seti, Name = "Seti" },
            new Operation { Action = Gtri, Name = "Gtri" },
            new Operation { Action = Gtir, Name = "Gtir" },
            new Operation { Action = Gtrr, Name = "Gtrr" },
            new Operation { Action = Eqir, Name = "Eqir" },
            new Operation { Action = Eqri, Name = "Eqri" },
            new Operation { Action = Eqrr, Name = "Eqrr" }
        };
        #endregion

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

        public static int Solve2(string input)
        {
            var deductions = Deductions(input);
            var reduced = ReduceDeductions(deductions);
            var operationsByOpCode = reduced.ToDictionary(kvp => kvp.Key, kvp => Operations.Single(op => op.Name == kvp.Value));

            var partTwoInput = input.Split("\n\n\n\n")[1].Trim();
            var instructions = partTwoInput.Split("\n").Select(s => s.Trim());
            var arguments = instructions.Select(i => i.ToInts(" "));
            var startingState = new[] { 0, 0, 0, 0 };
            foreach (var args in arguments)
            {
                var opCode = args[0];
                var operation = operationsByOpCode[opCode];
                operation.Action(startingState, args[1], args[2], args[3]);
            }

            return startingState[0];
        }

        public static Dictionary<int, string> ReduceDeductions(Dictionary<int, HashSet<string>> deductions)
        {
            var results = new Dictionary<int, string>();

            while (deductions.Any(d => d.Value.Count > 0))
            {
                var singleMatch = deductions.First(d => d.Value.Count == 1);
                var opCode = singleMatch.Key;
                var name = singleMatch.Value.Single();
                results[opCode] = name;

                foreach (var deduction in deductions)
                {
                    if (deduction.Value.Count > 0)
                    {
                        deduction.Value.Remove(name);
                    }
                }
            }

            return results;
        }

        public static Dictionary<int, HashSet<string>> Deductions(string input)
        {
            var partOneInput = input.Split("\n\n\n\n")[0];
            var samples = partOneInput.Split("\n\n");
            
            var opCodeMatches = new Dictionary<int, HashSet<string>>();
            for (var i = 0; i < Operations.Count; i++)
            {
                opCodeMatches[i] = Operations.Select(op => op.Name).ToHashSet();
            }

            foreach (var sample in samples)
            {
                var (startingRegisters, arguments, endingRegisters) = Parse(sample);
                var opCode = arguments[0];
                var matchingOperations = GetMatchingOperations(startingRegisters, arguments, endingRegisters).Select(op => op.Name).ToHashSet();

                var opCodePotentialMatches = opCodeMatches[opCode];
                var toRemove = new HashSet<string>();
                foreach (var potentialMatch in opCodePotentialMatches)
                {
                    if (!matchingOperations.Contains(potentialMatch))
                    {
                        toRemove.Add(potentialMatch);
                    }
                }

                foreach (var notAMatch in toRemove)
                {
                    opCodePotentialMatches.Remove(notAMatch);
                }
            }

            return opCodeMatches;
        }

        public static int CountMatchingOperations(string input)
        {
            var (startingRegisters, arguments, endingRegisters) = Parse(input);
            var matchingOperations = GetMatchingOperations(startingRegisters, arguments, endingRegisters);

            return matchingOperations.Count();
        }

        private static List<Operation> GetMatchingOperations(int[] startingRegisters, int[] arguments, int[] endingRegisters)
        {
            var operations = Operations.ToDictionary(op => op, op => true);

            foreach (var operation in Operations)
            {
                var isMatch = IsMatch(operation, startingRegisters, arguments, endingRegisters);

                if (!isMatch)
                {
                    operations[operation] = false;
                }
            }

            return operations.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        }

        public static (int[] startingRegisters, int[] arguments, int[] endingRegisters) Parse(string input)
        {
            var lines = input.Trim().Split("\n");
            var startingRegisters = ParseRegisters(lines[0]);
            var endingRegisters = ParseRegisters(lines[2]);
            var arguments = lines[1].Trim().ToInts(" ");
            return (startingRegisters, arguments, endingRegisters);
        }

        public static bool IsMatch(Operation operation, int[] startingRegisters, int[] arguments, int[] endingRegisters)
        {
            var newRegisters = Copy(startingRegisters);
            operation.Action(newRegisters, arguments[1], arguments[2], arguments[3]);

            return Equal(newRegisters, endingRegisters);
        }

        private static int[] ParseRegisters(string s)
        {
            return s
                .Replace("Before: [", "")
                .Replace("After:  [", "")
                .Replace("]", "")
                .Trim()
                .ToInts(", ");
        }

        private static int[] ToInts(this string s, string splitBy)
        {
            return s.Split(splitBy).Select(int.Parse).ToArray();
        }

        #region Operations

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

        #endregion

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

        public class Operation
        {
            public string Name { get; set; }
            public int OpCode { get; set; }
            public Action<int[], int, int, int> Action { get; set; }
        }
    }
}
