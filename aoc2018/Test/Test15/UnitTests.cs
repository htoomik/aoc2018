using Xunit;

namespace aoc2018.Test.Test15
{
    public class UnitTests
    {
        [Fact]
        public void ChooseTarget()
        {
            const string map = @"
#######
#E..G.#
#...#.#
#.G.#G#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var (target, routeLength) = elf.ChooseTarget();

            /*
             Expected:
                #######
                #E.+G.#
                #...#.#
                #.G.#G#
                #######
             */

            Assert.Equal(1, target.Row);
            Assert.Equal(3, target.Col);
            Assert.Equal(2, routeLength);
        }

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
            var (target, routeLength) = elf.ChooseTarget();
            var route = elf.ChooseRoute(target, routeLength);
            var routeEnd = route.Target;

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
