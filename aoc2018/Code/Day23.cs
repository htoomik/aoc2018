using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2018.Code
{
    public static class Day23
    {
        private static readonly Bot Origin = new Bot(0, 0, 0, 0);

        public static int Solve(string input)
        {
            var bots = Parse(input);
            var maxRange = bots.Max(b => b.R);
            var bestBot = bots.Single(b => b.R == maxRange);
            var inRange = bots.Count(b => InRangeOfBot(b, bestBot));
            return inRange;
        }

        public static (int distance, int count) Solve2(string input)
        {
            var bots = Parse(input);

            var blocks = SplitIntoBlocks(bots);
            
            var (bestBlocks, count) = FindBestBlock(blocks, bots);
            while (bestBlocks.First().R > 1)
            {
                var blocksToSearch = new List<Block>();
                foreach (var bestBlock in bestBlocks)
                {
                    blocks = SplitIntoBlocks(bestBlock);
                    blocksToSearch.AddRange(blocks);
                    
                }
                (bestBlocks, count) = FindBestBlock(blocksToSearch, bots);
            }

            var minDistance = bestBlocks.Min(b => DistanceBotToBlock(Origin, b));
            return (minDistance, count);
        }

        private static (List<Block>, int count) FindBestBlock(IEnumerable<Block> blocks, List<Bot> bots)
        {
            var bestCount = 0;
            var best = new List<Block>();

            foreach (var block in blocks)
            {
                var botsInRange = bots.Where(b => InRangeOfBlock(b, block)).ToList();
                var count = botsInRange.Count;
                if (count > bestCount)
                {
                    bestCount = count;
                    best = new List<Block> { block };
                }
                else if (count > 0 && count == bestCount)
                {
                    best.Add(block);
                }
            }

            return (best, bestCount);
        }

        private static List<Block> SplitIntoBlocks(List<Bot> bots)
        {
            var minX = bots.Min(b => b.X);
            var minY = bots.Min(b => b.Y);
            var minZ = bots.Min(b => b.Z);
            var maxX = bots.Max(b => b.X);
            var maxY = bots.Max(b => b.Y);
            var maxZ = bots.Max(b => b.Z);

            // Assume the sides are all roughly equal in magnitude
            // Split into roughly 1000 blocks
            var side = (maxX - minX + 1) / 10 + 1;

            var blocks = new List<Block>();
            for (var x = minX; x <= maxX; x += side)
            {
                for (var y = minY; y <= maxY; y+=side)
                {
                    for (var z = minZ; z <= maxZ; z += side)
                    {
                        blocks.Add(new Block(x, y, z, side));
                    }
                }
            }

            return blocks;
        }

        private static List<Block> SplitIntoBlocks(Block block)
        {
            var side = block.R / 10 + 1;

            var blocks = new List<Block>();
            for (var x = block.X; x <= block.X + block.R; x += side)
            {
                for (var y = block.Y; y <= block.Y + block.R; y += side)
                {
                    for (var z = block.Z; z <= block.Z + block.R; z += side)
                    {
                        blocks.Add(new Block(x, y, z, side));
                    }
                }
            }

            return blocks;
        }

        private static bool InRangeOfBot(Bot passive, Bot active)
        {
            return DistanceBotToBot(passive, active) <= active.R;
        }

        private static bool InRangeOfBlock(Bot bot, Block block)
        {
            return DistanceBotToBlock(bot, block) <= bot.R;
        }

        private static int DistanceBotToBot(Bot bot1, Bot bot2)
        {
            return Math.Abs(bot1.X - bot2.X) +
                   Math.Abs(bot1.Y - bot2.Y) +
                   Math.Abs(bot1.Z - bot2.Z);
        }       
        
        private static int DistanceBotToBlock(Bot bot, Block block)
        {
            // Get distance to closest face of the block
            // Don't count the dimensions where the bot is "inside" the block

            var xDistance = block.X < bot.X && bot.X < block.X + block.R
                ? 0
                : Math.Min(Math.Abs(block.X - bot.X), Math.Abs(block.X + block.R - 1 - bot.X));
            var yDistance = block.Y < bot.Y && bot.Y < block.Y + block.R
                ? 0
                : Math.Min(Math.Abs(block.Y - bot.Y), Math.Abs(block.Y + block.R - 1 - bot.Y));
            var zDistance = block.Z < bot.Z && bot.Z < block.Z + block.R
                ? 0
                : Math.Min(Math.Abs(block.Z - bot.Z), Math.Abs(block.Z + block.R - 1 - bot.Z));

            return xDistance + yDistance + zDistance;
        }

        private static List<Bot> Parse(string input)
        {
            var bots = new List<Bot>();
            var lines = input.Trim().Split("\n").Select(l => l.Trim());
            foreach (var line in lines)
            {
                var parts = line.Split(">, r=");
                var position = parts[0].Replace("pos=<", "").Split(",");
                var bot = new Bot(int.Parse(position[0]), int.Parse(position[1]), int.Parse(position[2]), int.Parse(parts[1]));
                bots.Add(bot);
            }

            return bots;
        }

        private struct Bot
        {
            public Bot(int x, int y, int z, int r)
            {
                X = x;
                Y = y;
                Z = z;
                R = r;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int R { get; }
        }

        private struct Block
        {
            public Block(int x, int y, int z, int r)
            {
                X = x;
                Y = y;
                Z = z;
                R = r;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int R { get; }
        }
    }
}
