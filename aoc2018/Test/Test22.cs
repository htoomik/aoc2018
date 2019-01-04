using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test22
    {
        private readonly ITestOutputHelper _output;
        
        public Test22(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 1, 0)]
        [InlineData(1, 1, 2)]
        [InlineData(10, 10, 0)]
        public void TestGetType(int x, int y, int expected)
        {
            var result = new Day22(10, 10, 510).GetType(x, y);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Test()
        {
            var result = new Day22(10, 10, 510).Solve();
            Assert.Equal(114, result);
        }

        [Fact]
        public void Solve()
        {
            var result = new Day22(13, 726, 3066).Solve();
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test2()
        {
            var time = new Day22(10, 10, 510, 20).FindShortestPath();
            Assert.Equal(45, time);
        }

        [Fact]
        public void Solve2()
        {
            var time = new Day22(13, 726, 3066, 20).FindShortestPath();
            _output.WriteLine(time.ToString());
        }
    }
}
