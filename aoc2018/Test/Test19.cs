using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test19
    {
        private readonly ITestOutputHelper _output;

        private const string Input = @"
#ip 0
seti 5 0 1
seti 6 0 2
addi 0 1 0
addr 1 2 3
setr 1 0 0
seti 8 0 4
seti 9 0 5";

        public Test19(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var result = Day19.Solve(Input.Trim().Split("\r\n").ToArray());
            var expected = new[] { 6, 5, 6, 0, 0, 9 };
            Assert.Equal(expected, result.state);
        }

        [Fact]
        public void Test2()
        {
            var result = Day19.Solve(Input.Trim().Split("\r\n").ToArray(), null, 2);
            var expected = new[] { 1, 5, 6, 0, 0, 0 };
            Assert.Equal(expected, result.state);
        }

        [Fact]
        public void Test3()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input19.txt");
            var start = new[] { 0, 1, 1251244, 1251244, 4, 10551345 };
            var result = Day19.Solve(input, start, 8);

            // ip in reg 4 has not been incremented yet but will be 4 again after incrementation
            var expected = new[] { 0, 1, 1251245, 1251245, 3, 10551345 };
            Assert.Equal(expected, result.state);
        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input19.txt");
            var result = Day19.Solve(input);
            _output.WriteLine(result.state[0].ToString());
            _output.WriteLine(result.iterations.ToString());
        }

        //[Fact]
        public void Solve2_a()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input19.txt");
            var result = Day19.Solve(input, new[] { 1, 0, 0, 0, 0, 0 }, printFrom: 10000, printTo: "2a");
            _output.WriteLine(string.Join(", ", result.state));
            _output.WriteLine(result.state[0].ToString());
        }

        [Fact]
        public void Solve2()
        {
            var result = Day19.Solve2();
            _output.WriteLine(result.Item1 + ", " + result.Item2);
        }
    }
}
