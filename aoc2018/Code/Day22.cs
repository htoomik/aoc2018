using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    public class Day22
    {
        private readonly int _maxX;
        private readonly int _maxY;
        private readonly int[,] _geoIndexes;
        private readonly int[,] _erosion;
        private readonly int[,] _types;
        private readonly int _targetX;
        private readonly int _targetY;
        private readonly int _depth;

        public Day22(int targetX, int targetY, int depth, int extra = 0 )
        {
            _maxX = targetX + 1 + extra;
            _maxY = targetY + 1 + extra;
            _geoIndexes = new int[_maxX, _maxY];
            _erosion = new int[_maxX, _maxY];
            _types = new int[_maxX, _maxY];
            _targetX = targetX;
            _targetY = targetY;
            _depth = depth;
        }

        private void Explore()
        {
            for (var y = 0; y < _maxY; y++)
            {
                for (var x = 0; x < _maxX; x++)
                {
                    var (gi, el, type) = Explore(x, y);
                    _geoIndexes[x, y] = gi;
                    _erosion[x, y] = el;
                    _types[x, y] = type;
                }
            }
        }

        private (int gi, int el, int type) Explore(int x, int y)
        {
            int geoIndex;
            if (x == 0 && y == 0)
                geoIndex = 0;
            else if (x == _targetX && y == _targetY)
                geoIndex = 0;
            else if (y == 0)
                geoIndex = x * 16807;
            else if (x == 0)
                geoIndex = y * 48271;
            else
            {
                var el1 = _erosion[x, y - 1];
                var el2 = _erosion[x - 1, y];
                geoIndex = el1 * el2;
            }

            var erosion = (geoIndex + _depth) % 20183;
            var type = erosion % 3;
            return (geoIndex, erosion, type);
        }

        public int GetType(int x, int y)
        {
            Explore();
            return _types[x, y];
        }

        public int Solve()
        {
            Explore();
            var sum = 0;
            for (int x = 0; x <= _targetX; x++)
            {
                for (int y = 0; y <= _targetY; y++)
                {
                    sum += GetType(x, y);
                }
            }

            return sum;
        }

        public int FindShortestPath()
        {
            Explore();

            var origin = new Choice(0, 0, Tool.Torch);
            var target = new Choice(_targetX, _targetY, Tool.Torch);

            var frontier = new PriorityQueue();
            frontier.Enqueue(origin, 0);

            var costs = new Dictionary<Choice, int> { [origin] = 0 };
            
            while (frontier.Count() != 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(target))
                    break;

                var choices = GetChoices(current);
                foreach (var next in choices)
                {
                    var costForMoving = CalculateCost(current, next);
                    var newCost = costs[current] + costForMoving;

                    if (!costs.ContainsKey(next) || newCost < costs[next])
                    {
                        costs[next] = newCost;
                        frontier.Enqueue(next, newCost);
                    }
                }
            }

            return costs[target];
        }

        private List<Choice> GetChoices(Choice current)
        {
            var choices = new List<Choice>();

            if (current.Y > 0)
            {
                var north = new Choice(current.X, current.Y - 1, current.T);
                if (Accessible(north))
                    choices.Add(north);
            }

            if (current.X > 0)
            {
                var west = new Choice(current.X - 1, current.Y, current.T);
                if (Accessible(west))
                    choices.Add(west);
            }

            if (current.X < _maxX - 1)
            {
                var east = new Choice(current.X + 1, current.Y, current.T);
                if (Accessible(east))
                    choices.Add(east);
            }

            if (current.Y < _maxY - 1)
            {
                var south = new Choice(current.X, current.Y + 1, current.T);
                if (Accessible(south))
                    choices.Add(south);
            }

            var switches = Enum.GetValues(typeof(Tool)).Cast<Tool>().Where(t => t != current.T);
            foreach (var switchTool in switches)
            {
                var choice = new Choice(current.X, current.Y, switchTool);
                if (Accessible(choice))
                    choices.Add(choice);
            }

            return choices;
        }

        private bool Accessible(Choice choice)
        {
            var type = _types[choice.X, choice.Y];

            switch (type)
            {
                case 0: // rocky
                    return choice.T == Tool.Rope || choice.T == Tool.Torch;
                case 1: // wet
                    return choice.T == Tool.Rope || choice.T == Tool.None;
                case 2: // narrow
                    return choice.T == Tool.Torch || choice.T == Tool.None;
            }
            throw new Exception();
        }

        private int CalculateCost(Choice from, Choice to)
        {
            return from.T == to.T ? 1 : 7;
        }

        private struct Choice
        {
            public Choice(int x, int y, Tool t)
            {
                X = x;
                Y = y;
                T = t;
            }

            public int X { get; }
            public int Y { get; }
            public Tool T { get; }
        }

        private enum Tool
        {
            None,
            Torch,
            Rope
        }

        private class PriorityQueue
        {
            private Dictionary<int, Queue<Choice>> _queue = new Dictionary<int, Queue<Choice>>();

            public void Enqueue(Choice choice, int cost)
            {
                if(!_queue.ContainsKey(cost))
                    _queue.Add(cost, new Queue<Choice>());

                _queue[cost].Enqueue(choice);
            }

            public int Count()
            {
                return _queue.Sum(q => q.Value.Count);
            }

            public Choice Dequeue()
            {
                var best = _queue.Keys.Min();
                var toReturn = _queue[best].Dequeue();
                if (_queue[best].Count == 0)
                    _queue.Remove(best);
                return toReturn;
            }
        }
    }
}
