using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day07
    {
        public static string Solve1(List<string> data)
        {
            var nodes = Parse(data, 0);

            var order = string.Empty;
            while (!nodes.All(n => n.Done))
            {
                var doable = nodes.Where(n => !n.Done && n.Doable);
                var first = doable.OrderBy(n => n.Name).First();
                order += first.Name;
                first.Done = true;
            }

            return order;
        }

        public static (string, int) Solve2(List<string> data, int workers, int extraTime)
        {
            var nodes = Parse(data, extraTime);

            var order = string.Empty;
            var second = 0;

            while (!nodes.All(n => n.Done))
            {
                var doing = nodes.Where(n => n.Doing);
                foreach (var node in doing)
                {
                    if (node.DoneBy(second))
                    {
                        order += node.Name;
                        node.Doing = false;
                        node.Done = true;
                    }
                }

                var doable = nodes.Where(n => !n.Done && !n.Doing && n.Doable).OrderBy(n => n.Name);
                var busyWorkers = nodes.Count(n => n.Doing);
                var freeWorkers = workers - busyWorkers;
                var canDo = doable.Take(freeWorkers);

                foreach (var node in canDo)
                {
                    node.StartAt(second);
                }

                second++;
            }

            return (order, second - 1);
        }

        private static List<Node> Parse(List<string> data, int extraTime)
        {
            var nodes = new Dictionary<char, Node>();
            foreach (var line in data)
            {
                var n1 = line[5];
                var n2 = line[36];

                if (!nodes.ContainsKey(n1))
                {
                    nodes[n1] = new Node { Name = n1, ExtraTime = extraTime };
                }
                if (!nodes.ContainsKey(n2))
                {
                    nodes[n2] = new Node { Name = n2, ExtraTime = extraTime };
                }

                var node1 = nodes[n1];
                var node2 = nodes[n2];

                var edge = new Edge { DoFirst = node1, ThenDo = node2 };
                node1.OutgoingEdges.Add(edge);
                node2.IncomingEdges.Add(edge);
            }

            return nodes.Values.ToList();
        }

        private class Node
        {
            public char Name { get; set; }
            public bool Doing { get; set; }
            public bool Done { get; set; }
            public int ExtraTime { get; set; }
            private int StartedAt { get; set; }
            private int TimeTaken => Name - 64 + ExtraTime;

            public readonly List<Edge> IncomingEdges = new List<Edge>();
            public readonly List<Edge> OutgoingEdges = new List<Edge>();

            public bool Doable 
            {
                get { return IncomingEdges.All(edge => edge.DoFirst.Done); }
            }

            public void StartAt(int second)
            {
                Doing = true;
                StartedAt = second;
            }

            public bool DoneBy(int second)
            {
                return second - StartedAt >= TimeTaken;
            }
        }

        private class Edge
        {
            public Node DoFirst { get; set; }
            public Node ThenDo { get; set; }
        }
    }
}
