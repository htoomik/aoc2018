using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    class Day08
    {
        public int Solve1(string data)
        {
            var nums = data.Split(" ").Select(int.Parse).ToList();
            var nodes = new List<Node>();

            Parse(nodes, nums, 0);
            
            return nodes.Sum(n => n.Metadata.Sum());
        }

        public int Solve2(string data)
        {
            var nums = data.Split(" ").Select(int.Parse).ToList();
            var nodes = new List<Node>();

            var root = Parse(nodes, nums, 0);
            var value = root.GetValue();

            return value;
        }

        private Node Parse(List<Node> nodes, List<int> nums, int pos)
        {
            var childCount = nums[pos];
            var metadataCount = nums[pos + 1];

            var node = new Node();
            nodes.Add(node);

            var nextChildStartsAt = pos + 2;

            for (var i = 0; i < childCount; i++)
            {
                var child = Parse(nodes, nums, nextChildStartsAt);
                node.Children.Add(child);
                nextChildStartsAt += child.Length;
            }

            for (var i = 0; i < metadataCount; i++)
            {
                node.Metadata.Add(nums[nextChildStartsAt + i]);
            }

            return node;
        }

        private class Node
        {
            public List<Node> Children { get; } = new List<Node>();
            public List<int> Metadata { get; } = new List<int>();

            public int Length => 2 + Children.Sum(c => c.Length) + Metadata.Count;

            public int GetValue()
            {
                var sum = 0;
                if (Children.Count == 0)
                {
                    sum = Metadata.Sum();
                }
                else
                {
                    foreach (var md in Metadata)
                    {
                        if (md <= Children.Count)
                        {
                            sum += Children[md - 1].GetValue();
                        }
                    }
                }

                return sum;
            }
        }
    }
}
