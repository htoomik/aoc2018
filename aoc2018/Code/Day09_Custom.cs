using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day09_Custom
    {
        public static long Solve(int players, int lastMarble)
        {
            var scores = InitializeScores(players);

            var marble = new Marble(0);
            marble.Previous = marble;
            marble.Next = marble;

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
                        marble = marble.Previous;
                    }
                    
                    scores[currentPlayer] += marble.Value;
                    var newCurrent = marble.Next;
                    marble.Previous.Next = marble.Next;
                    marble.Next.Previous = marble.Previous;
                    marble = newCurrent;
                }
                else
                {
                    var newMarble = new Marble(i);
                    var insertAfter = marble.Next;
                    newMarble.Previous = insertAfter;
                    newMarble.Next = insertAfter.Next;
                    insertAfter.Next = newMarble;
                    newMarble.Next.Previous = newMarble;

                    marble = newMarble;
                }
            }

            return scores.Values.Max();
        }

        private static Dictionary<int, long> InitializeScores(int players)
        {
            var scores = new Dictionary<int, long>();
            for (var i = 1; i <= players; i++)
            {
                scores[i] = 0;
            }

            return scores;
        }

        private class Marble
        {
            public int Value { get; set; }
            public Marble Next { get; set; }
            public Marble Previous { get; set; }

            public Marble(int value)
            {
                Value = value;
            }
        }
    }
}
