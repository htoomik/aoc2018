using System.Collections.Generic;

namespace aoc2018.Code.Day15
{
    public class Parser
    {
        public static (bool[,], List<Unit>) Parse(List<string> input, Engine engine)
        {
            var walls = new bool[input.Count, input[0].Length];
            var units = new List<Unit>();

            for (var i = 0; i < input.Count; i++)
            {
                var line = input[i];
                for (var j = 0; j < line.Length; j++)
                {
                    switch (line[j])
                    {
                        case '#':
                            walls[i, j] = true;
                            break;
                        case 'E':
                            units.Add(new Unit(i, j, Race.Elf, engine));
                            break;
                        case 'G':
                            units.Add(new Unit(i, j, Race.Goblin, engine));
                            break;
                    }
                }
            }

            return (walls, units);
        }
    }
}
