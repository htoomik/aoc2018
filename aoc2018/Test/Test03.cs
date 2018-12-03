using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test03
    {
        private readonly ITestOutputHelper _output;

        public Test03(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2",
            };
            Assert.Equal(4, Day03.Solve1(data));
        }
        
        [Fact]
        public void Solve1()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input03.txt").ToList();
            _output.WriteLine("Solution: " + Day03.Solve1(data));
        }

        [Fact]
        public void Test2()
        {
            var data = new List<string>
            {
                "#1 @ 1,3: 4x4",
                "#2 @ 3,1: 4x4",
                "#3 @ 5,5: 2x2",
            };
            Assert.Equal(3, Day03.Solve2(data));
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input03.txt").ToList();
            _output.WriteLine("Solution: " + Day03.Solve2(data));
        }
    }
}
