using System.IO;
using System.Linq;
using System.Text;
using aoc2018.Code;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test
{
    public class Test25
    {
        private readonly ITestOutputHelper _output;
        
        public Test25(ITestOutputHelper output)
        {
            _output = output;
        }

        private const string Input1 = @"
 0,0,0,0
 3,0,0,0
 0,3,0,0
 0,0,3,0
 0,0,0,3
 0,0,0,6
 9,0,0,0
12,0,0,0";

        private const string Input2 = @"
-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0";

        private const string Input3 = @"
1,-1,0,1
2,0,-1,0
3,2,-1,0
0,0,3,1
0,0,-1,-1
2,3,-2,0
-2,2,0,0
2,-2,0,-1
1,-1,0,-1
3,2,0,2";

        private const string Input4 = @"
1,-1,-1,-2
-2,-2,0,1
0,2,1,3
-2,3,-2,1
0,2,3,-2
-1,-1,1,-2
0,-2,-1,0
-2,2,3,-1
1,2,2,0
-1,-2,0,-2";

        [Theory]
        [InlineData(Input1, 2)]
        [InlineData(Input2, 4)]
        [InlineData(Input3, 3)]
        [InlineData(Input4, 8)]
        public void Test(string input, int expected)
        {
            var result = Day25.Solve(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Solve()
        {
            var input = File.ReadAllText("C:\\Code\\aoc2018\\aoc2018\\Data\\input25.txt");
            var result = Day25.Solve(input);
            _output.WriteLine(result.ToString());
        }
    }
}
