using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test11
    {
        private readonly ITestOutputHelper _output;

        public Test11(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(3, 5, 8, 4)]
        [InlineData(122, 79, 57, -5)]
        [InlineData(217, 196, 39, 0)]
        [InlineData(101, 153, 71, 4)]
        public void TestPowerLevel(int x, int y, int serial, int expected)
        {
            var result = Day11.GetPowerLevel(x, y, serial);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(18, 33, 45, 29)]
        [InlineData(42, 21, 61, 30)]
        public void Test1(int serial, int expectedX, int expectedY, int expectedMax)
        {
            var result = Day11.Solve1(serial);
            Assert.Equal(expectedX, result.X);
            Assert.Equal(expectedY, result.Y);
            Assert.Equal(expectedMax, result.Level);
        }
        
        [Theory]
        [InlineData(18, 90, 269, 16, 113)]
        [InlineData(42, 232, 251, 12, 119)]
        public void Test2(int serial, int expectedX, int expectedY, int expectedSize, int expectedMax)
        {
            var (cell, size) = Day11.Solve2(serial);
            Assert.Equal(expectedX, cell.X);
            Assert.Equal(expectedY, cell.Y);
            Assert.Equal(expectedMax, cell.Level);
            Assert.Equal(expectedSize, size);
        }
        
        [Fact]
        public void Solve1()
        {
            var result = Day11.Solve1(4151);
            _output.WriteLine(result.ToString());
        }
        
        [Fact]
        public void Solve2()
        {
            var (cell, size) = Day11.Solve2(4151);
            _output.WriteLine($"X: {cell.X}, Y: {cell.Y}, Level: {cell.Level}, Size: {size}");
            _output.WriteLine($"{cell.X},{cell.Y},{size}");
        }
    }
}
