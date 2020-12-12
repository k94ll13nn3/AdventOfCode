using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day12 : Day
    {
        public override string Title => "Rain Risk";

        public override string ProcessFirst()
        {
            IEnumerable<(char dir, int steps)> moves = GetLines().Select(x => (dir: x[0], steps: int.Parse(x[1..])));
            int direction = 1;
            (int x, int y) position = (0, 0);
            foreach ((char dir, int steps) in moves)
            {
                switch (dir)
                {
                    case 'N':
                        position.x -= steps;
                        break;

                    case 'S':
                        position.x += steps;
                        break;

                    case 'W':
                        position.y -= steps;
                        break;

                    case 'E':
                        position.y += steps;
                        break;

                    case 'L':
                        direction = (((direction - (steps / 90)) % 4) + 4) % 4;
                        break;

                    case 'R':
                        direction = (direction + (steps / 90)) % 4;
                        break;

                    case 'F':
                        switch (direction)
                        {
                            case 0:
                                position.x -= steps;
                                break;

                            case 2:
                                position.x += steps;
                                break;

                            case 3:
                                position.y -= steps;
                                break;

                            case 1:
                                position.y += steps;
                                break;
                        }
                        break;
                }
            }

            return $"{Math.Abs(position.x) + Math.Abs(position.y)}";
        }

        public override string ProcessSecond()
        {
            IEnumerable<(char dir, int steps)> moves = GetLines().Select(x => (dir: x[0], steps: int.Parse(x[1..])));
            (int x, int y) position = (0, 0);
            (int x, int y) waypoint = (-1, 10);
            foreach ((char dir, int steps) in moves)
            {
                switch (dir)
                {
                    case 'N':
                        waypoint.x -= steps;
                        break;

                    case 'S':
                        waypoint.x += steps;
                        break;

                    case 'W':
                        waypoint.y -= steps;
                        break;

                    case 'E':
                        waypoint.y += steps;
                        break;

                    case 'R':
                        for (int i = 0; i < (steps / 90); i++)
                        {
                            waypoint = (waypoint.y, waypoint.x * -1);
                        }
                        break;

                    case 'L':
                        for (int i = 0; i < (steps / 90); i++)
                        {
                            waypoint = (waypoint.y * -1, waypoint.x);
                        }
                        break;

                    case 'F':
                        for (int i = 0; i < steps; i++)
                        {
                            position = (waypoint.x + position.x, waypoint.y + position.y);
                        }
                        break;
                }
            }

            return $"{Math.Abs(position.x) + Math.Abs(position.y)}";
        }
    }
}
