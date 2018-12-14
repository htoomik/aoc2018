using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day12
    {
        public static long Solve1(List<string> input, long generations)
        {
            var (plants, rules) = Parse(input);

            long sum = 0;
            long previousSum = 0;
            var recentDiffs = new List<long> { 1, 2, 3, 4, 5 };
            var stabilized = false;

            int i;
            for (i = 0; i < generations; i++)
            {
                plants = Spread(plants, rules);
                sum = Sum(plants);
                var diff = sum - previousSum;
                previousSum = sum;
                recentDiffs.RemoveAt(0);
                recentDiffs.Add(diff);

                if (recentDiffs.Distinct().Count() == 1)
                {
                    stabilized = true;
                    break;
                }
            }

            if (stabilized)
            {
                var remainingIterations = generations - i - 1;
                sum += remainingIterations * recentDiffs[0];
            }

            return sum;
        }

        private static int Sum(Dictionary<int, bool> plants)
        {
            var sum = 0;
            foreach (var plant in plants)
            {
                if (plant.Value)
                    sum += plant.Key;
            }

            return sum;
        }

        private static (Dictionary<int, bool>, List<Rule>) Parse(List<string> input)
        {
            var initialState = new Dictionary<int, bool>();

            var stateLine = input[0];
            var state = stateLine.Split(": ")[1];
            for (var i = 0; i < state.Length; i++)
            {
                initialState[i] = state[i] == '#';
            }

            var ruleLines = input.Skip(2);
            var rules = ruleLines.Select(ParseRule).ToList();

            return (initialState, rules);
        }

        public static Rule ParseRule(string input)
        {
            var parts = input.Split(" => ");

            var condition = new bool[5];
            var key = 0;
            for (var i = 0; i < 5; i++)
            {
                var b = parts[0][i] == '#';
                condition[i] = b;
                key = key  | (b ? 1 << i : 0);
            }

            var outcome = parts[1] == "#";

            return new Rule { Outcome = outcome, Key = key };
        }

        private static Dictionary<int, bool> Spread(Dictionary<int, bool> plants, List<Rule> rules)
        {
            var newPlants = new Dictionary<int, bool>();
            var minIndex = plants.Keys.Min() - 2;
            var maxIndex = plants.Keys.Max() + 2;
            for (var i = minIndex; i <= maxIndex; i++)
            {
                var state = GetState(plants, i);
                var matchingRule = FindRule(state, rules);
                if (matchingRule != null && matchingRule.Value.Outcome)
                {
                    newPlants[i] = true;
                }
            }

            return newPlants;
        }

        private static Rule? FindRule(int state, List<Rule> rules)
        {
            return rules.SingleOrDefault(r => r.Key == state);
        }

        public static int GetState(Dictionary<int, bool> plants, int i)
        {
            var state = 0;
            for (var j = 0; j < 5; j++)
            {
                var b = plants.ContainsKey(i + j - 2) && plants[i + j - 2];
                var flag = b ? 1 << j : 0;
                state = state | flag;
            }
            return state;
        }

        public struct Rule
        {
            public int Key { get; set; }
            public bool Outcome { get; set; }
        }
    }
}
