using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day11 : Day
    {
        public override string Title => "Chronal Charge";

        public override string ProcessFirst()
        {
            var serial = 7315;
            var max = 0;
            var map = new int[300, 300];
            var coord = (x: 0, y: 0);
            for (int i = 1; i < 299; i++)
            {
                for (int j = 1; j < 299; j++)
                {
                    map[i, j] = int.MinValue;
                }
            }

            for (int i = 1; i < 299; i++)
            {
                for (int j = 1; j < 299; j++)
                {
                    var sum = GetValue(i - 1, j - 1) + GetValue(i, j - 1) + GetValue(i + 1, j - 1) +
                        GetValue(i - 1, j + 1) + GetValue(i, j + 1) + GetValue(i + 1, j + 1) +
                        GetValue(i - 1, j) + GetValue(i, j) + GetValue(i + 1, j);

                    if (sum > max)
                    {
                        coord = (i - 1, j - 1); // top left coord
                        max = sum;
                    }
                }
            }

            return $"{coord.x},{coord.y}";

            int GetValue(int x, int y)
            {
                if (map[x, y] == int.MinValue)
                {
                    var rackId = x + 10;
                    map[x, y] = (int)Math.Abs(((rackId * y) + serial) * rackId / 100 % 10) - 5;
                }

                return map[x, y];
            }
        }

        public override string ProcessSecond()
        {
            var serial = 7315;
            int max = 0;
            var map = new int[300, 300];
            var coord = (x: 0, y: 0, size: 0);
            for (int i = 1; i < 299; i++)
            {
                for (int j = 1; j < 299; j++)
                {
                    map[i, j] = int.MinValue;
                }
            }
            
            for (int i = 0; i <= 299; i++)
            {
                for (int j = 0; j <= 299; j++)
                {
                    int sum = GetValue(i, j);
                    for (int k = 1; k <= 299 - Math.Max(i, j); k++)
                    {
                        for (int t = i; t <= i + k - 1; t++)
                        {
                            sum += GetValue(t, j + k);
                        }

                        for (int t = j; t <= j + k - 1; t++)
                        {
                            sum += GetValue(i + k, t);
                        }

                        sum += GetValue(i + k, j + k);

                        if (sum > max)
                        {
                            coord = (i, j, k + 1); // +1 because the k is the number of points after the first (k is 1 for a 2x2 square)
                            max = sum;
                        }
                    }
                }
            }
            
            return $"{coord.x},{coord.y},{coord.size}";

            int GetValue(int x, int y)
            {
                if (map[x, y] == int.MinValue)
                {
                    var rackId = x + 10;
                    map[x, y] = (int)Math.Abs(((rackId * y) + serial) * rackId / 100 % 10) - 5;
                }

                return map[x, y];
            }
        }
    }
}