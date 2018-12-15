using System.Linq;
using aoc2018.Code.Day15;
using Xunit;

namespace aoc2018.Test.Test15
{
    public class ParserTests
    {
        [Fact]
        public void Test()
        {
            const string input = @"
#######
#.G.E.#
#E.G.E#
#.G.E.#
#######";
            var list = input.Split("\r\n").Skip(1).ToList();
            var (walls, units) = Parser.Parse(list, null);

            Assert.Equal(5, walls.GetLength(0));
            Assert.Equal(7, walls.GetLength(1));
            Assert.True(walls[0, 0]);
            Assert.True(walls[1, 0]);
            Assert.True(walls[0, 1]);
            Assert.True(walls[4, 0]);
            Assert.True(walls[0, 6]);

            Assert.Equal(7, units.Count);
            Assert.Equal(3, units.Count(u => u.Race == Race.Goblin));
            Assert.Equal(4, units.Count(u => u.Race == Race.Elf));

            var sorted = units.InReadingOrder();

            var firt = sorted[0];
            Assert.Equal(1, firt.Row);
            Assert.Equal(2, firt.Col);
            Assert.Equal(Race.Goblin, firt.Race);
        }
    }
}
