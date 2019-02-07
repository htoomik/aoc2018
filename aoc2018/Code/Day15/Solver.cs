using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2018.Code.Day15
{
    class Solver
    {
        public int Solve2(List<string> map, string print = null)
        {
            var elfPower = Engine.DefaultAttackPower;
            while (true)
            {
                var engine = new Engine();
                engine.Initialize(map, elfPower);

                var startingElfCount = engine.Units.Count(u => u.Race == Race.Elf);

                engine.RunGame();

                if (print != null)
                {
                    PrintFinalState(print, engine);
                }

                var elvesWon = engine.Units[0].Race == Race.Elf;
                if (elvesWon)
                {
                    var elfCount = engine.Units.Count(u => u.Race == Race.Elf);
                    var elvesLost = startingElfCount - elfCount;
                    if (elvesLost <= 0)
                    {
                        return GetFinalOutcome(engine);
                    }
                }

                elfPower++;
            }
        }
        
        public int Solve1(List<string> map, string print = null)
        {
            var engine = new Engine();
            engine.Initialize(map);
            engine.RunGame();

            if (print != null)
            {
                PrintFinalState(print, engine);
            }

            var outcome = GetFinalOutcome(engine);

            return outcome;
        }

        private static void PrintFinalState(string print, Engine engine)
        {
            var final = new StringBuilder();
            final.AppendLine("Ended after " + engine.Rounds);
            final.AppendLine("Health left " + engine.Units.Sum(u => u.HitPoints));
            final.AppendLine(GetFinalOutcome(engine).ToString());
            File.AppendAllText(print, final.ToString());
        }

        private static int GetFinalOutcome(Engine engine)
        {
            var rounds = engine.Rounds;
            var hitPointsLeft = engine.Units.Sum(u => u.HitPoints);
            var outcome = rounds * hitPointsLeft;
            return outcome;
        }
    }
}
