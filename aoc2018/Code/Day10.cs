using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    class Day10
    {
        public static int Solve1(List<string> data, out string result)
        {
            var lights = data.Select(Parse).ToList();

            var seconds = 0;
            int? allPassedOrigin = null;
            while (!Done(lights))
            {
                seconds++;
                lights.ForEach(p => p.Move());

                if (lights.All(l => l.PassedOrigin) && !allPassedOrigin.HasValue)
                {
                    allPassedOrigin = seconds;
                }

                if (allPassedOrigin.HasValue && seconds - allPassedOrigin > 20)
                {
                    Print(lights, "ex");
                    throw new Exception("All lights have crossed origin and still no solution");
                }
            }

            result = Print(lights);

            return seconds;
        }

        public static int Solve2(List<string> data)
        {
            return 0;
        }

        private static bool Done(List<Light> lights)
        {
            var width = lights.Max(p => p.PosX) - lights.Min(p => p.PosX) + 1;
            var height = lights.Max(p => p.PosY) - lights.Min(p => p.PosY) + 1;
            if (width > lights.Count)
                return false;
            if (height > lights.Count)
                return false;
            
            foreach (var light in lights)
            {
                var neighbors = lights.Where(l => IsNeighbor(l, light));

                if (!neighbors.Any())
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsNeighbor(Light l1, Light l2)
        {
            var xNeighbor = Math.Abs(l1.PosX - l2.PosX) == 1 &&
                             Math.Abs(l1.PosY - l2.PosY) <= 1;
            var yNeighbor = Math.Abs(l1.PosX - l2.PosX) <= 1 &&
                            Math.Abs(l1.PosY - l2.PosY) == 1;
            var corner = Math.Abs(l1.PosX - l2.PosX) == 1 &&
                         Math.Abs(l1.PosY - l2.PosY) == 1;
            return xNeighbor || yNeighbor || corner;
        }

        private static string Print(List<Light> lights, string suffix = null)
        {
            var minX = lights.Min(p => p.PosX);
            var maxX = lights.Max(p => p.PosX);
            var minY = lights.Min(p => p.PosY);
            var maxY = lights.Max(p => p.PosY);
            var width = maxX - minX + 1;
            var height = maxY - minY + 1;

            var sky = new char[width, height];
            
            foreach (var light in lights)
            {
                var stdX = light.PosX - minX;
                var stdY = light.PosY - minY;
                sky[stdX, stdY] = '#';
            }

            var sb = new StringBuilder();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    sb.Append(sky[x, y] == 0 ? "." : "#");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static Light Parse(string line)
        {
            // position=< 9,  1> velocity=< 0,  2>
            var posAndVel = line.Split("> velocity=<");
            
            var pos = posAndVel[0].Substring(10);
            var posXAndY = pos.Split(",");

            var vel = posAndVel[1].Replace(">", "");
            var velXAndY = vel.Split(",");

            return new Light(
                posX: int.Parse(posXAndY[0].Trim()),
                posY: int.Parse(posXAndY[1].Trim()),
                velX: int.Parse(velXAndY[0].Trim()),
                velY: int.Parse(velXAndY[1].Trim())
            );
        }

        private class Light
        {
            public bool PassedOrigin { get; set; }

            private int OriginalX { get; }
            private int OriginalY { get; }

            public int PosX { get; set; }
            public int PosY { get; set; }
            public int VelX { get; set; }
            public int VelY { get; set; }

            public Light(int posX, int posY, int velX, int velY)
            {
                PosX = posX;
                PosY = posY;
                VelX = velX;
                VelY = velY;

                OriginalX = posX;
                OriginalY = posY;
            }

            public void Move()
            {
                PosX += VelX;
                PosY += VelY;

                if (Math.Sign(PosX) != Math.Sign(OriginalX))
                    PassedOrigin = true;
                if (Math.Sign(PosY) != Math.Sign(OriginalY))
                    PassedOrigin = true;
            }
        }

        private enum Doneness
        {
            Yes,
            No,
            Maybe
        }
    }
}
