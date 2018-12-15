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
        public void ChooseNextStep()
        {
            const string map = @"
#######
#.E...#
#.....#
#...G.#
#######";
            var engine = Helper.CreateEngine(map);
            var elf = Helper.TopLeftElf(engine);
            var (target, routeLength) = elf.ChooseTarget();
            var nextStep = engine.ChooseNextStepTowards(elf, target, routeLength);

            /*
             Expected:
                In range:     Nearest:      Chosen:       Distance:     Step:
                #######       #######       #######       #######       #######
                #.E...#       #.E...#       #.E...#       #4E212#       #..E..#
                #...?.#  -->  #...!.#  -->  #...+.#  -->  #32101#  -->  #.....#
                #..?G?#       #..!G.#       #...G.#       #432G2#       #...G.#
                #######       #######       #######       #######       #######
             */

            Assert.Equal(1, nextStep.Row);
            Assert.Equal(3, nextStep.Col);
        }
    }
}
