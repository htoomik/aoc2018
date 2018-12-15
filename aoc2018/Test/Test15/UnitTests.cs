using Xunit;

namespace aoc2018.Test.Test15
{
    public class UnitTests
    {
        [Fact]
        public void Move()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var route = elf.ChooseRoute();
            var routeEnd = route.End;

            /*
             Expected:
                #######
                #E.+G.#
                #...#.#
                #.G.#G#
                #######
             */

            Assert.Equal(1, routeEnd.Row);
            Assert.Equal(3, routeEnd.Col);
        }
    }
}
