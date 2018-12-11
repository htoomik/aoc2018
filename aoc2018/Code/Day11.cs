namespace aoc2018.Code
{
    class Day11
    {
        private const int GridSize = 300;

        public static Cell Solve1(int serial)
        {
            var grid = GenerateGrid(serial);
            var max = FindMax(grid, 3);
            return max;
        }

        public static (Cell cell, int size) Solve2(int serial)
        {
            var grid = GenerateGrid(serial);
            Cell bestResult = null;
            var bestSize = 0;

            for (var size = 1; size <= 300; size++)
            {
                var cell = FindMax(grid, size);
                if (bestResult == null || cell.Level > bestResult.Level)
                {
                    bestResult = cell;
                    bestSize = size;
                }

                // result plotted against size has a bellish shape; once it's down to zero we can quit
                if (cell.Level == 0 && size > 10)
                {
                    break;
                }
            }
            return (bestResult, bestSize);
        }

        public static int GetPowerLevel(int x, int y, int serial)
        {
            var rackId = x + 10;
            var step = (rackId * y + serial) * rackId;
            var hundreds = step / 100 % 10;
            var powerLevel = hundreds - 5;
            return powerLevel;
        }

        private static int[,] GenerateGrid(int serial)
        {
            var grid = new int[GridSize + 1, GridSize + 1];
            for (var x = 1; x <= GridSize; x++)
            {
                for (var y = 1; y <= GridSize; y++)
                {
                    grid[x, y] = GetPowerLevel(x, y, serial);
                }
            }

            return grid;
        }

        private static Cell FindMax(int[,] grid, int size)
        {
            var bestX = 0;
            var bestY = 0;
            var max = 0;
            for (var i = 1; i <= GridSize - size; i++)
            {
                for (var j = 1; j <= GridSize - size; j++)
                {
                    var level = 0;
                    for (int k = 0; k < size; k++)
                    {
                        for (int m = 0; m < size; m++)
                        {
                            level += grid[i + k, j + m];
                        }
                    }

                    if (level > max)
                    {
                        bestX = i;
                        bestY = j;
                        max = level;
                    }
                }    
            }

            return new Cell { X = bestX, Y = bestY, Level = max };
        }

        public class Cell
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Level { get; set; }
        }
    }
}
