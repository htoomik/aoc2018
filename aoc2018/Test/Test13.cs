using System.IO;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test13
    {
        private readonly ITestOutputHelper _output;

        public Test13(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1a()
        {
            var input = @"
|
v
|
|
|
^
|";
            var result = new Day13().Solve1(input);
            Assert.Equal(0, result.Col);
            Assert.Equal(3, result.Row);
        }

        [Fact]
        public void Test1b()
        {
            var input = @"
/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ";
            var result = new Day13().Solve1(input);
            Assert.Equal(3, result.Row);
            Assert.Equal(7, result.Col);
        }

        [Fact]
        public void Solve1()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input13.txt");
            input = input.Replace("\n", "\r\n");
            var result = new Day13().Solve1(input);
            _output.WriteLine($"Row: {result.Row}, Col: {result.Col}");
        }

        [Fact]
        public void Test2()
        {
            var input = @"
/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/";
            var result = new Day13().Solve2(input);
            Assert.Equal(4, result.Row);
            Assert.Equal(6, result.Col);
        }

        
        [Fact]
        public void Solve2()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input13.txt");
            input = input.Replace("\n", "\r\n");
            var result = new Day13().Solve2(input);
            _output.WriteLine($"Row: {result.Row}, Col: {result.Col}");
        }

    }
}
