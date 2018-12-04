using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test04
    {
        private readonly ITestOutputHelper _output;

        public Test04(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var data = new List<string>
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up",
            };
            var result = Day04.Solve1(data);
            Assert.Equal(10, result.Item1);
            Assert.Equal(24, result.Item2);
        }
        
        [Fact]
        public void Solve1()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input04.txt").ToList();
            var result = Day04.Solve1(data);
            var total = result.Item1 * result.Item2;
            _output.WriteLine(total.ToString());
        }

        [Fact]
        public void Test2()
        {
            var data = new List<string>
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up",
            };
            var result = Day04.Solve2(data);
            Assert.Equal(99, result.Item1);
            Assert.Equal(45, result.Item2);
        }

        [Fact]
        public void Solve2()
        {
            var data = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input04.txt").ToList();
            var result = Day04.Solve2(data);
            var total = result.Item1 * result.Item2;
            _output.WriteLine(total.ToString());
        }
    }
}
