using System.IO;
using System.Linq;
using aoc2018.Code.Day15;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test.Test15
{
    public class Solution
    {
        private readonly ITestOutputHelper _output;

        public Solution(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Solve1()
        {
            var map = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input15.txt").ToList();
            var outcome = new Solver().Solve1(map);

            _output.WriteLine(outcome.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var map = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input15.txt").ToList();
            var solution = new Solver().Solve2(map);
            _output.WriteLine(solution.ToString());
        }
    }
}
