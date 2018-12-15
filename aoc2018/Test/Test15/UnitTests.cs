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
            var canMove = elf.ChooseTarget(out var target, out var routeLength);

            /*
             Expected:
                #######
                #E.+G.#
                #...#.#
                #.G.#G#
                #######
             */

            Assert.True(canMove);
            Assert.Equal(1, target.Value.Row);
            Assert.Equal(3, target.Value.Col);
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
            elf.ChooseTarget(out var target, out var routeLength);
            var nextStep = engine.ChooseNextStepTowards(elf, target.Value, routeLength.Value);

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
