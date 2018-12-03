using System.Collections.Generic;

namespace aoc2018.Code
{
    public class Day01
    {
        public static int FindFirstRepeatedValue(List<int> data)
        {
            var frequencies = new HashSet<int>();
            var current = 0;
            
            while (true)
            {
                foreach (var change in data)
                {
                    current += change;
                    if (frequencies.Contains(current))
                        return current;
                    else
                        frequencies.Add(current);
                }
            }
        }
    }
}
