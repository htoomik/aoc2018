using System.Linq;
using aoc2018.Code.Day15;

namespace aoc2018.Test.Test15
{
    internal static class Helper
    {
        public static Engine CreateEngine(string map)
        {
            var input = map.Split("\r\n").Skip(1).ToList();
            var engine = new Engine();
            engine.Initialize(input);
            return engine;
        }

        public static Unit TopLeftElf(Engine engine, int index = 0)
        {
            var elves = engine.Units.Where(u => u.Race == Race.Elf);
            var attackingElf = elves.TopLeftUnit(index);
            return attackingElf;
        }
    }
}
