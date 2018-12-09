using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day09_List
    {
        public static int Solve(int players, int lastMarble)
        {
            var scores = InitializeScores(players);
            var circle = new List<int> { 0 };

            var currentPos = 0;
            var currentPlayer = 1;

            for (var i = 1; i <= lastMarble; i++)
            {
                currentPlayer ++;
                if (currentPlayer > players)
                    currentPlayer = 1;

                if (i % 23 == 0)
                {
                    scores[currentPlayer] += i;
                    currentPos -= 7;
                    if (currentPos < 0)
                        currentPos += circle.Count;

                    var toRemove = circle[currentPos];
                    circle.Remove(toRemove);
                    scores[currentPlayer] += toRemove;
                }
                else
                {
                    currentPos += 2;
                    currentPos = currentPos % circle.Count;
                    circle.Insert(currentPos, i);
                }
            }

            return scores.Values.Max();
        }

        private static Dictionary<int, int> InitializeScores(int players)
        {
            var scores = new Dictionary<int, int>();
            for (var i = 1; i <= players; i++)
            {
                scores[i] = 0;
            }

            return scores;
        }
    }
}
