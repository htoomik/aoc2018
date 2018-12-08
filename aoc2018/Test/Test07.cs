using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test07
    {
        private readonly ITestOutputHelper _output;

        public Test07(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = new List<string>
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin.",
            };
            var result = Day07.Solve1(data);
            Assert.Equal("CABDFE", result);
        }
        
        [Fact]
        public void Solve1()
        {
            Trace.Listeners.Add(new DefaultTraceListener());
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input07.txt").ToList();
            var result = Day07.Solve1(data);
            _output.WriteLine(result);
        }

        [Fact]
        public void Test2()
        {
            var data = new List<string>
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin.",
            };
            var result = Day07.Solve2(data, 2, 0);
            Assert.Equal("CABFDE", result.Item1);
            Assert.Equal(15, result.Item2);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input07.txt").ToList();
            var result = Day07.Solve2(data, 5, 60);
            _output.WriteLine(result.ToString());
        }
    }
}
