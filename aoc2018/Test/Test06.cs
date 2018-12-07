using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test06
    {
        private readonly ITestOutputHelper _output;

        public Test06(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = new List<string>
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9",
            };
            var result = Day06.Solve1(data);
            Assert.Equal(17, result);
        }
        
        [Fact]
        public void Solve1()
        {
            Trace.Listeners.Add(new DefaultTraceListener());
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input06.txt").ToList();
            var result = Day06.Solve1(data);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test2()
        {
            var data = new List<string>
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9",
            };
            var result = Day06.Solve2(data, 32);
            Assert.Equal(16, result);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input06.txt").ToList();
            var result = Day06.Solve2(data, 10000);
            _output.WriteLine(result.ToString());
        }
    }
}
