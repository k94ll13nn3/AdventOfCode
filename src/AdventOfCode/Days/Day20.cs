using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day20 : Day
    {
        public override string Title => "Donut Maze";

        public override string ProcessFirst()
        {
            string[] map = GetLines().ToArray();
            var doors = new Dictionary<(int x, int y), string>();
            for (int i = 2; i < map.Length - 2; i++)
            {
                for (int j = 2; j < map[0].Length - 2; j++)
                {
                    if (map[i][j] is '.')
                    {
                        if (map[i - 1][j] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = $"{map[i - 2][j]}{map[i - 1][j]}";
                        }

                        if (map[i + 1][j] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = $"{map[i + 1][j]}{map[i + 2][j]}";
                        }

                        if (map[i][j - 1] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = $"{map[i][j - 2]}{map[i][j - 1]}";
                        }

                        if (map[i][j + 1] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = $"{map[i][j + 1]}{map[i][j + 2]}";
                        }
                    }
                }
            }

            (int x, int y) start = doors.First(x => x.Value == "AA").Key;
            (int x, int y) end = doors.First(x => x.Value == "ZZ").Key;

            return FindShortestPath(start, map, 0, end, 0, new(), doors).ToString();
        }

        public override string ProcessSecond()
        {
            string[] map = GetLines().ToArray();
            var doors = new Dictionary<(int x, int y), (bool outer, string door)>();
            for (int i = 2; i < map.Length - 2; i++)
            {
                for (int j = 2; j < map[0].Length - 2; j++)
                {
                    if (map[i][j] is '.')
                    {
                        if (map[i - 1][j] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = (i - 2 == 0, $"{map[i - 2][j]}{map[i - 1][j]}");
                        }

                        if (map[i + 1][j] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = (i + 2 == map.Length - 1, $"{map[i + 1][j]}{map[i + 2][j]}");
                        }

                        if (map[i][j - 1] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = (j - 2 == 0, $"{map[i][j - 2]}{map[i][j - 1]}");
                        }

                        if (map[i][j + 1] is >= 'A' and <= 'Z')
                        {
                            doors[(i, j)] = (j + 2 == map[0].Length - 1, $"{map[i][j + 1]}{map[i][j + 2]}");
                        }
                    }
                }
            }

            (int x, int y) start = doors.First(x => x.Value.door == "AA").Key;
            (int x, int y) end = doors.First(x => x.Value.door == "ZZ").Key;

            var t = new Queue<((int x, int y) position, HashSet<(int x, int y, int level)> seenPositions, int steps, int level)>();
            t.Enqueue((start, new(), 0, 0));

            return FindShortestPastWithLevels(t, map, end, doors, (doors.Keys.Count - 2) / 2).ToString();
        }

        private static (int x, int y, int direction) GetPosition((int x, int y) position, string[] map, int direction)
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

        private static int FindShortestPastWithLevels(Queue<((int x, int y) position, HashSet<(int x, int y, int level)> seenPositions, int steps, int level)> points, string[] map, (int x, int y) end, Dictionary<(int x, int y), (bool outer, string door)> doors, int maxLevel)
        {
            ((int x, int y) position, HashSet<(int x, int y, int level)> seenPositions, int steps, int level) current = points.Dequeue();
            while (current.level != 0 || current.position != end)
            {
                if (current.level < maxLevel)
                {
                    var newPositions = new HashSet<(int x, int y, int level)>(current.seenPositions)
                    {
                        (current.position.x, current.position.y, current.level)
                    };

                    int pC = points.Count;
                    if (doors.ContainsKey(current.position) && doors[current.position].door != "AA" && current.position != end)
                    {
                        (int x, int y) door = doors.First(x => x.Value.door == doors[current.position].door && x.Key != current.position).Key;
                        if (!doors[current.position].outer)
                        {
                            if (!newPositions.Contains((door.x, door.y, current.level + 1)))
                            {
                                points.Enqueue((door, newPositions, current.steps + 1, current.level + 1));
                            }
                        }
                        else if (current.level >= 1 && !newPositions.Contains((door.x, door.y, current.level - 1)))
                        {
                            points.Enqueue((door, newPositions, current.steps + 1, current.level - 1));
                        }
                    }

                    if (pC == points.Count)
                    {
                        List<(int x, int y, int direction)> possiblePositions = new();
                        possiblePositions.Add(GetPosition(current.position, map, 0));
                        possiblePositions.Add(GetPosition(current.position, map, 1));
                        possiblePositions.Add(GetPosition(current.position, map, 2));
                        possiblePositions.Add(GetPosition(current.position, map, 3));

                        foreach ((int x, int y, int direction) in possiblePositions.Where(p => !newPositions.Contains((p.x, p.y, current.level)) && map[p.x][p.y] is '.'))
                        {
                            points.Enqueue(((x, y), newPositions, current.steps + 1, current.level));
                        }
                    }
                }

                current = points.Dequeue();
            }
            return current.steps;
        }

        private int FindShortestPath((int x, int y) position, string[] map, int direction, (int x, int y) end, int steps, HashSet<(int x, int y)> seenPositions, Dictionary<(int x, int y), string> doors)
        {
            if (position == end)
            {
                return steps;
            }

            var newPositions = new HashSet<(int x, int y)>(seenPositions)
            {
                position
            };

            if (doors.ContainsKey(position) && doors[position] != "AA" && position != end)
            {
                (int x, int y) door = doors.First(x => x.Value == doors[position] && x.Key != position).Key;
                if (!newPositions.Contains(door))
                {
                    return FindShortestPath(door, map, direction, end, steps + 1, newPositions, doors);
                }
            }

            List<(int x, int y, int direction)> possiblePositions = new();
            possiblePositions.Add(GetPosition(position, map, 0));
            possiblePositions.Add(GetPosition(position, map, 1));
            possiblePositions.Add(GetPosition(position, map, 2));
            possiblePositions.Add(GetPosition(position, map, 3));

            var b = new ConcurrentBag<int> { int.MaxValue };
            foreach ((int x, int y, int direction) possiblePosition in possiblePositions.Where(p => !newPositions.Contains((p.x, p.y)) && map[p.x][p.y] is '.'))
            {
                b.Add(FindShortestPath((possiblePosition.x, possiblePosition.y), map, possiblePosition.direction, end, steps + 1, newPositions, doors));
            }

            return b.Min();
        }
    }
}
