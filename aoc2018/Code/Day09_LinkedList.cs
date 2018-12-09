using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day09_LinkedList
    {
        public static int Solve(int players, int lastMarble)
        {
            var scores = InitializeScores(players);
            var circle = new LinkedList<int>();

            var current = circle.AddFirst(0);
            var currentPlayer = 1;

            for (var i = 1; i <= lastMarble; i++)
            {
                currentPlayer ++;
                if (currentPlayer > players)
                    currentPlayer = 1;

                if (i % 23 == 0)
                {
                    scores[currentPlayer] += i;
                    for (var j = 0; j < 7; j++)
                    {
                        current = current.Previous ?? circle.Last;
                    }

                    var toRemove = current.Value;
                    current = current.Next ?? circle.First;
                    circle.Remove(toRemove);
                    scores[currentPlayer] += toRemove;
                }
                else
                {
                    var n1 = current.Next ?? circle.First;
                    current = circle.AddAfter(n1, i);
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
