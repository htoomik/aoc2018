using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test01
    {
        private readonly ITestOutputHelper _output;

        public Test01(ITestOutputHelper output)
        {
            this._output = output;
        }


        [Fact]
        public void Test()
        {
            var data = new List<int> { +7, +7, -2, -7, -4 };
            Assert.Equal(14, Day01.FindFirstRepeatedValue(data));
        }

        [Fact]
        public void Solve()
        {
            var lines = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input01.txt");
            var data = lines.Select(int.Parse).ToList();
            _output.WriteLine("Solution: " + Day01.FindFirstRepeatedValue(data));
        }
    }
}
