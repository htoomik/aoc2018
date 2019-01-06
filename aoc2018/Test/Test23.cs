using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test23
    {
        private readonly ITestOutputHelper _output;
        
        public Test23(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test()
        {
            var input = @"
pos=<0,0,0>, r=4
pos=<1,0,0>, r=1
pos=<4,0,0>, r=3
pos=<0,2,0>, r=1
pos=<0,5,0>, r=3
pos=<0,0,3>, r=1
pos=<1,1,1>, r=1
pos=<1,1,2>, r=1
pos=<1,3,1>, r=1";
            var result = Day23.Solve(input.Trim());
            Assert.Equal(7, result);
        }

        [Fact]
        public void Test2()
        {
            var input = @"
pos=<10,12,12>, r=2
pos=<12,14,12>, r=2
pos=<16,12,12>, r=4
pos=<14,14,14>, r=6
pos=<50,50,50>, r=200
pos=<10,10,10>, r=5";
            var (distance, count) = Day23.Solve2(input.Trim());
            Assert.Equal(36, distance);
            Assert.Equal(5, count);
        }

        [Fact]
        public void Test2_a()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input23_a.txt");
            var (distance, count) = Day23.Solve2(input);
            Assert.Equal(975, count);

        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input23.txt");
            var result = Day23.Solve(input);
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input23.txt");
            var result = Day23.Solve2(input);
            _output.WriteLine(result.ToString());
        }
    }
}
