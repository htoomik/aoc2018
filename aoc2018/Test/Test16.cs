using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test16
    {
        private readonly ITestOutputHelper _output;

        private const string Input = @"
Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]";

        public Test16(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void MatchesMulr()
        {
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(Input.Trim());
            var isMatch = Day16.IsMatch(new Day16.Operation { Action = Day16.Mulr }, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void MatchesAddi()
        {
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(Input.Trim());
            var isMatch = Day16.IsMatch(new Day16.Operation { Action = Day16.Addi }, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void MatchesSeti()
        {
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(Input.Trim());
            var isMatch = Day16.IsMatch(new Day16.Operation { Action = Day16.Seti }, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void Test1()
        {
            var output = Day16.CountMatchingOperations(Input.Trim());
            Assert.Equal(3, output);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input16.txt");
            var count = Day16.Solve1(input);
            _output.WriteLine(count.ToString());
        }

        [Fact]
        public void GetDeductions()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input16.txt");
            var deductions = Day16.Deductions(input);
            foreach (var deduction in deductions)
            {
                _output.WriteLine(deduction.Key + " " + string.Join(",", deduction.Value));
            }
        }
 
        [Fact]
        public void ReduceDeductions()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input16.txt");
            var deductions = Day16.Deductions(input);
            var reduced = Day16.ReduceDeductions(deductions);
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input16.txt");
            var answer = Day16.Solve2(input);
            _output.WriteLine(answer.ToString());
        }
    }
}
