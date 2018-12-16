using System.IO;
using System.Linq;
using aoc2018.Code.Day15;
using Xunit;
using Xunit.Abstractions;

namespace aoc2018.Test.Test15
{
    public class Solution
    {
        private readonly ITestOutputHelper _output;

        public Solution(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Solve1()
        {
            var map = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input15.txt").ToList();
            var engine = new Engine();
            engine.Initialize(map);
            engine.RunGame();

            var rounds = engine.Rounds;
            var hitPointsLeft = engine.Units.Sum(u => u.HitPoints);

            var outcome = rounds * hitPointsLeft;

            _output.WriteLine(outcome.ToString());
        }

        [Fact]
        public void Solve2()
        {
            var map = File.ReadAllLines("C:\\Code\\aoc2018\\aoc2018\\Data\\input15.txt").ToList();
            const string outFile = "C:\\Code\\aoc2018\\output15_1.txt";
            var elfPower = Engine.DefaultAttackPower + 14;
            while (true)
            {
                Output(outFile, "Trying elf power " + elfPower);
                var engine = new Engine();
                engine.Initialize(map, elfPower);

                var startingElfCount = engine.Units.Count(u => u.Race == Race.Elf);

                engine.RunGame();

                var elvesWon = engine.Units[0].Race == Race.Elf;
                if (!elvesWon)
                {
                    Output(outFile, "Elves lost");
                }
                else
                {
                    var elfCount = engine.Units.Count(u => u.Race == Race.Elf);
                    var elvesLost = startingElfCount - elfCount;
                    if (elvesLost > 0)
                    {
                        Output(outFile, $"Elves won, but {elvesLost} elves died.");
                    }
                    else
                    {
                        Output(outFile, "Elves won, no elves died.");
                        break;
                    }
                }

                elfPower++;
            }
        }

        private void Output(string path, string message)
        {
            _output.WriteLine(message);
            File.AppendAllText(path, message + "\r\n");
        }
    }
}
