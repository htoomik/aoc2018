using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test12
    {
        private readonly ITestOutputHelper _output;

        public Test12(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1()
        {
            var input = @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";
            var list = input.Split("\r\n").ToList();
            var result = Day12.Solve1(list, 20);
            Assert.Equal(325, result);
        }

        [Theory]
        [InlineData("...## => #", 24, true)]
        [InlineData("#.#.# => .", 21, false)]
        public void RuleParsing(string input, int key, bool outcome)
        {
            var rule = Day12.ParseRule(input);
            Assert.Equal(outcome, rule.Outcome);
            Assert.Equal(key, rule.Key);
        }

        [Fact]
        public void GetState()
        {
            // Plants in pots 1, 2, 4
            // Looking at pot 3
            // Expecting "##.#." or 11

            var plants = new Dictionary<int, bool> { { 1, true }, { 2, true }, { 4, true } };
            var state = Day12.GetState(plants, 3);
            Assert.Equal(11, state);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input12.txt").ToList();
            var result = Day12.Solve1(input, 20);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input12.txt").ToList();
            var result = Day12.Solve1(input, 50000000000);
            _output.WriteLine(result.ToString());
        }
    }
}
