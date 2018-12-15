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
        public void Solve()
        {
            var map = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input15.txt").ToList();
            var engine = new Engine();
            engine.Initialize(map);
            engine.RunGame();

            var rounds = engine.Rounds;
            var hitPointsLeft = engine.Units.Sum(u => u.HitPoints);

            var outcome = rounds * hitPointsLeft;

            _output.WriteLine(outcome.ToString());
        }
    }
}
