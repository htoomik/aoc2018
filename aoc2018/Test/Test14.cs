using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test14
    {
        private readonly ITestOutputHelper _output;

        public Test14(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(9, "5158916779")]
        [InlineData(5, "0124515891")]
        [InlineData(18, "9251071085")]
        [InlineData(2018, "5941429882")]
        public void Test1(int input, string expected)
        {
            Assert.Equal(expected, Day14.Solve1(input));
        }

        [Fact]
        public void Solve1()
        {
            var result = Day14.Solve1(165061);
            _output.WriteLine(result);
        }

        [Theory]
        [InlineData("51589", 9)]
        [InlineData("01245", 5)]
        [InlineData("92510", 18)]
        [InlineData("59414", 2018)]
        public void Test2(string input, int expected)
        {
            var result = Day14.Solve2(input);
            Assert.Equal(expected, result.Item2);
        }

        [Fact]
        public void Solve2()
        {
            var result = Day14.Solve2("165061");
            File.WriteAllText("C:\\Code\\aoc2018\\output14.txt", result.Item1);
            _output.WriteLine(result.Item2.ToString());
        }
    }
}
