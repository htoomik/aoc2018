using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test02
    {
        private readonly ITestOutputHelper _output;

        public Test02(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = new List<string>
            {
                "abcdef",
                "bababc",
                "abbcde",
                "abcccd",
                "aabcdd",
                "abcdee",
                "ababab",
            };
            Assert.Equal(12, Day02.Solve1(data));
        }

        [Fact]
        public void Solve1()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input02.txt").ToList();
            _output.WriteLine("Solution: " + Day02.Solve1(data));
        }

        [Fact]
        public void Test2()
        {
            var data = new List<string>
            {
                "abcde",
                "fghij",
                "klmno",
                "pqrst",
                "fguij",
                "axcye",
                "wvxyz",
            };
            var solution = Day02.Solve2(data);
            Assert.Equal("fghij", solution.Item1);
            Assert.Equal("fguij", solution.Item2);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input02.txt").ToList();
            _output.WriteLine("Solution: " + Day02.Solve2(data));
        }

    }
}
