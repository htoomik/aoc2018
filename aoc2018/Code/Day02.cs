using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day02
    {
        public static int Solve1(List<string> data)
        {
            var twos = 0;
            var threes = 0;

            foreach (var d in data)
            {
                var chars = d.ToCharArray();
                var counts = chars.GroupBy(c => c);
                if (counts.Any(c => c.Count() == 2))
                {
                    twos++;
                }

                if (counts.Any(c => c.Count() == 3))
                {
                    threes++;
                }
            }

            return twos * threes;
        }

        public static (string, string) Solve2(List<string> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = i + 1; j < data.Count; j++)
                {
                    var diffCount = 0;
                    var string1 = data[i];
                    var string2 = data[j];

                    for (int k = 0; k < string1.Length; k++)
                    {
                        if (string1[k] != string2[k])
                        {
                            diffCount++;
                        }
                    }

                    if (diffCount == 1)
                    {
                        return (string1, string2);
                    }
                }
            }

            return (null, null);
        }
    }
}
