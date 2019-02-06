using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test21
    {
        private readonly ITestOutputHelper _output;

        public Test21(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Solve_1()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input21.txt");
            var result = Day21.Solve(input, firstPart: true);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Solve_2()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input21.txt");
            var result = Day21.Solve(input, firstPart: false);
            _output.WriteLine(result.ToString());
        }
    }
}
