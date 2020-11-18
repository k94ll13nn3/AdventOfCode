using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day18 : Day
    {
        public override string Title => "Many-Worlds Interpretation";

        public override string ProcessFirst()
        {
            // Not fast enough!
            char[][] map = GetLines().Select(x => x.ToCharArray()).ToArray();
            int ind = Array.FindIndex(map, x => Array.IndexOf(x, '@') >= 0);
            (int x, int y) start = (ind, Array.IndexOf(map[ind], '@'));

            return FindShortestPath(start, map, -1, 0, new(), 16).ToString();
        }

        public override string ProcessSecond()
        {
            return 0.ToString();
        }

        private static (int x, int y, int direction) GetPosition((int x, int y) position, char[][] map, int direction)
        {
            if (position.x + 1 < map.Length && direction == 1)
            {
                return (position.x + 1, position.y, 1);
            }

            if (position.x - 1 >= 0 && direction == 3)
            {
                return (position.x - 1, position.y, 3);
            }

            if (position.y + 1 < map[0].Length && direction == 2)
            {
                return (position.x, position.y + 1, 2);
            }

            if (position.y - 1 >= 0 && direction == 0)
            {
                return (position.x, position.y - 1, 0);
            }

            return (-1, -1, -1);
        }

        private int FindShortestPath((int x, int y) position, char[][] map, int direction, int steps, HashSet<char> keys, int numberOfKeys)
        {
            if (map[position.x][position.y] is >= 'A' and <= 'Z' && !keys.Contains((char)(map[position.x][position.y] + 32)))
            {
                return int.MaxValue;
            }

            var newKeys = keys.ToHashSet();
            if (map[position.x][position.y] is >= 'a' and <= 'z' && !newKeys.Contains(map[position.x][position.y]))
            {
                direction = -1;
                newKeys.Add(map[position.x][position.y]);
            }

            if (newKeys.Count == numberOfKeys)
            {
                return steps;
            }

            List<(int x, int y, int direction)> possiblePositions = new();
            if (direction == -1)
            {
                possiblePositions.Add(GetPosition(position, map, 0));
                possiblePositions.Add(GetPosition(position, map, 1));
                possiblePositions.Add(GetPosition(position, map, 2));
                possiblePositions.Add(GetPosition(position, map, 3));
            }
            else
            {
                possiblePositions.Add(GetPosition(position, map, direction));
                possiblePositions.Add(GetPosition(position, map, (direction + 1) % 4));
                possiblePositions.Add(GetPosition(position, map, (direction + 3) % 4));
            }

            var b = new ConcurrentBag<int> { int.MaxValue };
            IEnumerable<(int x, int y, int direction)> t = possiblePositions.Where(p => p.direction != -1 && map[p.x][p.y] is not '#');
            if (t.Any())
            {
                Parallel.ForEach(
                    t,
                    possiblePosition => b.Add(FindShortestPath((possiblePosition.x, possiblePosition.y), map, possiblePosition.direction, steps + 1, newKeys, numberOfKeys)));
            }

            return b.Min();
        }
    }
}
