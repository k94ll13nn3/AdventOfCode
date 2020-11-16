using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day3 : Day
    {
        public override string Title => "Crossed Wires";

        public override string ProcessFirst()
        {
            return Compute().distance.ToString();
        }

        public override string ProcessSecond()
        {
            return Compute().steps.ToString();
        }

        private (int distance, int steps) Compute()
        {
            var lines = GetLines().ToList();
            IEnumerable<(char direction, int distance)> firstWire = lines[0].Split(',').Select(x => (x[0], int.Parse(x[1..])));
            IEnumerable<(char direction, int distance)> secondWire = lines[1].Split(',').Select(x => (x[0], int.Parse(x[1..])));

            const int center = 20000;
            (char wireCode, int step)[,] map = new (char wireCode, int step)[center * 2, center * 2];
            map[center, center] = ('X', 0);

            (int x, int y) position;
            char wireCode = 'a';
            int minDistance = int.MaxValue;
            int steps = int.MaxValue;
            foreach (IEnumerable<(char direction, int distance)> wire in new[] { firstWire, secondWire })
            {
                position = (center, center);
                int step = 0;
                foreach ((char direction, int distance) in wire)
                {
                    for (int i = 1; i <= distance; i++)
                    {
                        switch (direction)
                        {
                            case 'U':
                                position.x--;
                                break;

                            case 'D':
                                position.x++;
                                break;

                            case 'R':
                                position.y++;
                                break;

                            case 'L':
                                position.y--;
                                break;
                        }

                        step++;
                        if (wireCode == 'b' && map[position.x, position.y].wireCode == 'a')
                        {
                            steps = Math.Min(steps, map[position.x, position.y].step + step);
                            int currentDistance = Math.Abs(position.x - center) + Math.Abs(position.y - center);
                            minDistance = Math.Min(minDistance, currentDistance);
                        }

                        map[position.x, position.y] = (wireCode, step);
                    }
                }

                wireCode = 'b';
            }

            return (minDistance, steps);
        }
    }
}
