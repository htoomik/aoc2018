using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test05
    {
        private readonly ITestOutputHelper _output;

        public Test05(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = "dabAcCaCBAcCcaDA";
            var result = Day05.Solve1(data);
            Assert.Equal(10, result);
        }
        
        [Fact]
        public void Solve1()
        {
            var data = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input05.txt").Trim();
            var result = Day05.Solve1(data);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test2()
        {
            var data = "dabAcCaCBAcCcaDA";
            var result = Day05.Solve2(data);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input05.txt").Trim();
            var result = Day05.Solve2(data);
            _output.WriteLine(result.ToString());
        }
    }
}
