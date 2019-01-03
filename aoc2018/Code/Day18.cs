using System;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc2018.Code
{
    public class Day18
    {
        public static (string state, int answer, int repeat) Solve(string input, int minutes)
        {
            var array = Parse(input);
            var i1000 = "";
            var repeat = 0;

            for (var i = 0; i < minutes; i++)
            {
                array = Mutate(array);

                if (i == 1000)
                {
                    i1000 = Print(array);
                }

                if (i > 1000 && repeat == 0)
                {
                    var temp = Print(array);
                    if (temp == i1000)
                    {
                        repeat = i;
                    }
                }
            }

            var state = Print(array);
            var value = Evaluate(array);
            return (state, value, repeat);
        }

        private static char[,] Parse(string input)
        {
            var lines = input.Trim().Split("\n").Select(l => l.Trim()).ToArray();
            var array = new char[lines.Length, lines[0].Length];

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (var j = 0; j < line.Length; j++)
                {
                    array[i, j] = line[j];
                }
            }

            return array;
        }

        private static char[,] Mutate(char[,] array)
        {
            var newArray = new char[array.GetLength(0), array.GetLength(1)];

            // Skip edges to begin with
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    var currentValue = array[i, j];
                    var trees = CountSurrounding(array, i, j, '|');
                    var yards = CountSurrounding(array, i, j, '#');
                    char newValue;

                    switch (currentValue)
                    {
                        case '.':
                        {
                            newValue = trees >= 3 ? '|' : '.';
                            break;
                        }
                        case '|':
                        {
                            newValue = yards >= 3 ? '#' : '|';
                            break;
                        }
                        case '#':
                        {
                            newValue = yards >= 1 && trees >= 1 ? '#' : '.';
                            break;
                        }
                        default:
                            throw new Exception("Unexpected character");
                    }

                    newArray[i, j] = newValue;
                }
            }

            return newArray;
        }

        private static int CountSurrounding(char[,] array, int i, int j, char c)
        {
            var count = 0;
            for (var k = -1; k < 2; k++)
            {
                for (var m = -1; m < 2; m++)
                {
                    if (k == 0 && m == 0)
                        continue;

                    if (i + k < 0 || i + k >= array.GetLength(0))
                        continue;

                    if (j + m < 0 || j + m >= array.GetLength(1))
                        continue;

                    if (array[i + k, j + m] == c)
                        count++;
                }
            }

            return count;
        }

        private static string Print(char[,] array)
        {
            var s = new StringBuilder();
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    s.Append(array[i, j]);
                }

                s.AppendLine();
            }

            return s.ToString().Trim();
        }

        private static int Evaluate(char[,] array)
        {
            var woods = 0;
            var yards = 0;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    var c = array[i, j];

                    if (c == '|')
                        woods++;
                    if (c == '#')
                        yards++;
                }
            }

            return woods * yards;
        }
    }
}
