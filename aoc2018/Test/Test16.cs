using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test16
    {
        private readonly ITestOutputHelper _output;

        private const string input = @"
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
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(input.Trim());
            var isMatch = Day16.IsMatch(Day16.Mulr, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void MatchesAddi()
        {
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(input.Trim());
            var isMatch = Day16.IsMatch(Day16.Addi, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void MatchesSeti()
        {
            var (startingRegisters, arguments, endingRegisters) = Day16.Parse(input.Trim());
            var isMatch = Day16.IsMatch(Day16.Seti, startingRegisters, arguments, endingRegisters);
            Assert.True(isMatch);
        }

        [Fact]
        public void Test1()
        {
            var output = Day16.CountMatchingOperations(input.Trim());
            Assert.Equal(3, output);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input16.txt");
            var count = Day16.Solve1(input);
            _output.WriteLine(count.ToString());
        }
    }
}
