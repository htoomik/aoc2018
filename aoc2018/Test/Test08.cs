using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test08
    {
        private readonly ITestOutputHelper _output;

        public Test08(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            var result = new Day08().Solve1(data);
            Assert.Equal(138, result);
        }
        
        [Fact]
        public void Solve1()
        {
            var data = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input08.txt");
            var result = new Day08().Solve1(data);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test2()
        {
            var data = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
            var result = new Day08().Solve2(data);
            Assert.Equal(66, result);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input08.txt");
            var result = new Day08().Solve2(data);
            _output.WriteLine(result.ToString());
        }
    }
}
