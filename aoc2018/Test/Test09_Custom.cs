using System.Diagnostics;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test09_Custom
    {
        private readonly ITestOutputHelper _output;

        public Test09_Custom(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(9, 25, 32)]
        [InlineData(10, 1618, 8317)]
        [InlineData(13, 7999, 146373)]
        [InlineData(17, 1104, 2764)]
        [InlineData(21, 6111, 54718)]
        [InlineData(30, 5807, 37305)]
        public void Test1(int players, int lastMarble, int highScore)
        {
            var result = Day09_Custom.Solve(players, lastMarble);
            Assert.Equal(highScore, result);
        }
        
        [Fact]
        public void Solve1()
        {
            Trace.Listeners.Add(new DefaultTraceListener());
            var result = Day09_Custom.Solve(400, 71864);
            _output.WriteLine(result.ToString());
        }
        
        [Fact]
        public void Solve2()
        {
            Trace.Listeners.Add(new DefaultTraceListener());
            var result = Day09_Custom.Solve(400, 7186400);
            _output.WriteLine(result.ToString());
        }
    }
}
