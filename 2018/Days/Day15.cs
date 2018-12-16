using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : Day
    {
        public override string Title => "Beverage Bandits";

        public override string ProcessFirst()
        {
            var map = GetLinesAsStrings().Select(x => x.Select(y => new Square(y)).ToList()).ToList();
            var idd = 0;
            while (true)
            {
                var unitsOrder = new List<Point>();
                for (int i = 0; i < map.Count; i++)
                {
                    for (int j = 0; j < map[i].Count; j++)
                    {
                        if (map[i][j].Unit != null && !unitsOrder.Contains(new Point(i, j, map[i][j].Unit.IsElf)))
                        {
                            unitsOrder.Add(new Point(i, j, map[i][j].Unit.IsElf));
                        }
                    }
                }

                if (unitsOrder.All(x => x.IsElf) || unitsOrder.All(x => !x.IsElf))
                {
                    return $"{(idd)}/{unitsOrder.Select(s => map[s.X][s.Y].Unit.Life).Sum()}/{(idd) * unitsOrder.Select(s => map[s.X][s.Y].Unit.Life).Sum()}";
                }

                foreach (var unit in unitsOrder)
                {
                    if (map[unit.X][unit.Y].Unit == null)
                    {
                        continue;
                    }

                    if (unitsOrder.Where(x => map[x.X][x.Y].Unit != null).All(x => map[x.X][x.Y].Unit.IsElf)
                        || unitsOrder.Where(x => map[x.X][x.Y].Unit != null).All(x => !map[x.X][x.Y].Unit.IsElf))
                    {
                        foreach (var tt in unitsOrder)
                        {
                            System.Console.Write($"{map[tt.X][tt.Y].Content}({map[tt.X][tt.Y].Unit?.Life ?? -1}) ");
                        }
                        System.Console.WriteLine(idd);
                        var sum = unitsOrder.Where(x => map[x.X][x.Y].Unit != null).Select(s => map[s.X][s.Y].Unit.Life).Sum();
                        var turns = idd;
                        return $"{turns}/{sum}/{turns * sum}";
                    }

                    if ((map[unit.X][unit.Y - 1].Unit == null || map[unit.X][unit.Y - 1].Unit.IsElf == unit.IsElf)
                        && (map[unit.X][unit.Y + 1].Unit == null || map[unit.X][unit.Y + 1].Unit.IsElf == unit.IsElf)
                        && (map[unit.X + 1][unit.Y].Unit == null || map[unit.X + 1][unit.Y].Unit.IsElf == unit.IsElf)
                        && (map[unit.X - 1][unit.Y].Unit == null || map[unit.X - 1][unit.Y].Unit.IsElf == unit.IsElf))
                    {
                        var res = FindMinimumDistance(map, new Neighboor(unit.X, unit.Y, 0, null), new HashSet<int>(unitsOrder.Where(u => u.IsElf != unit.IsElf).Select(u => 1000 * u.X + u.Y)));
                        var minDistance = res.d;
                        var next = res.p;

                        if (next != (0, 0))
                        {
                            map[next.x][next.y].Content = map[unit.X][unit.Y].Content;
                            map[next.x][next.y].Unit = map[unit.X][unit.Y].Unit;
                            map[unit.X][unit.Y].Content = '.';
                            map[unit.X][unit.Y].Unit = null;

                            unit.X = next.x;
                            unit.Y = next.y;
                        }
                    }

                    var minLife =
                    Math.Min(
                        Math.Min(
                            map[unit.X - 1][unit.Y].Unit != null && map[unit.X - 1][unit.Y].Unit.IsElf != unit.IsElf ? map[unit.X - 1][unit.Y].Unit.Life : 201,
                            map[unit.X][unit.Y - 1].Unit != null && map[unit.X][unit.Y - 1].Unit.IsElf != unit.IsElf ? map[unit.X][unit.Y - 1].Unit.Life : 201),
                        Math.Min(
                            map[unit.X][unit.Y + 1].Unit != null && map[unit.X][unit.Y + 1].Unit.IsElf != unit.IsElf ? map[unit.X][unit.Y + 1].Unit.Life : 201,
                            map[unit.X + 1][unit.Y].Unit != null && map[unit.X + 1][unit.Y].Unit.IsElf != unit.IsElf ? map[unit.X + 1][unit.Y].Unit.Life : 201)
                    );

                    if (map[unit.X - 1][unit.Y].Unit != null && map[unit.X - 1][unit.Y].Unit.IsElf != unit.IsElf && map[unit.X - 1][unit.Y].Unit.Life == minLife)
                    {
                        map[unit.X - 1][unit.Y].Unit.Life -= 3;
                        if (map[unit.X - 1][unit.Y].Unit.Life <= 0)
                        {
                            map[unit.X - 1][unit.Y].Unit = null;
                            map[unit.X - 1][unit.Y].Content = '.';
                        }
                    }
                    else if (map[unit.X][unit.Y - 1].Unit != null && map[unit.X][unit.Y - 1].Unit.IsElf != unit.IsElf && map[unit.X][unit.Y - 1].Unit.Life == minLife)
                    {
                        map[unit.X][unit.Y - 1].Unit.Life -= 3;
                        if (map[unit.X][unit.Y - 1].Unit.Life <= 0)
                        {
                            map[unit.X][unit.Y - 1].Unit = null;
                            map[unit.X][unit.Y - 1].Content = '.';
                        }
                    }
                    else if (map[unit.X][unit.Y + 1].Unit != null && map[unit.X][unit.Y + 1].Unit.IsElf != unit.IsElf && map[unit.X][unit.Y + 1].Unit.Life == minLife)
                    {
                        map[unit.X][unit.Y + 1].Unit.Life -= 3;
                        if (map[unit.X][unit.Y + 1].Unit.Life <= 0)
                        {
                            map[unit.X][unit.Y + 1].Unit = null;
                            map[unit.X][unit.Y + 1].Content = '.';
                        }
                    }
                    else if (map[unit.X + 1][unit.Y].Unit != null && map[unit.X + 1][unit.Y].Unit.IsElf != unit.IsElf && map[unit.X + 1][unit.Y].Unit.Life == minLife)
                    {
                        map[unit.X + 1][unit.Y].Unit.Life -= 3;
                        if (map[unit.X + 1][unit.Y].Unit.Life <= 0)
                        {
                            map[unit.X + 1][unit.Y].Unit = null;
                            map[unit.X + 1][unit.Y].Content = '.';
                        }
                    }
                }

                idd++;

                foreach (var tt in unitsOrder)
                {
                    System.Console.Write($"{map[tt.X][tt.Y].Content}({map[tt.X][tt.Y].Unit?.Life ?? -1}) ");
                }
                System.Console.WriteLine(idd);
            }
        }

        private (int d, (int x, int y) p) FindMinimumDistance(List<List<Square>> map, Neighboor start, HashSet<int> ends)
        {
            var stack = new Queue<Neighboor>();
            stack.Enqueue(start);
            return FindMinimumDistanceImpl(stack, new HashSet<int>());

            (int, (int x, int y)) FindMinimumDistanceImpl(Queue<Neighboor> points, HashSet<int> visitedPoints)
            {
                if (points.Count == 0)
                {
                    return (int.MaxValue, (0, 0));
                }

                var point = points.Dequeue();
                if (ends.Contains(point.X * 1000 + point.Y))
                {
                    var t = point;
                    while (t.Previous != null && t.Previous.Previous != null)
                    {
                        t = t.Previous;
                    }

                    return (point.Distance - 1, (t.X, t.Y));
                }

                if (point != start && map[point.X][point.Y].Content != '.')
                {
                    return FindMinimumDistanceImpl(points, visitedPoints);
                }

                var tan = new Queue<Neighboor>(points);

                if (point.X > 1 && map[point.X - 1][point.Y].Content != '#')
                {
                    var nextPoint = new Neighboor(point.X - 1, point.Y, point.Distance + 1, point);
                    if (!visitedPoints.Contains(nextPoint.X * 1000 + nextPoint.Y))
                    {
                        visitedPoints.Add(nextPoint.X * 1000 + nextPoint.Y);
                        tan.Enqueue(nextPoint);
                    }
                }
                if (point.Y > 1 && map[point.X][point.Y - 1].Content != '#')
                {
                    var nextPoint = new Neighboor(point.X, point.Y - 1, point.Distance + 1, point);
                    if (!visitedPoints.Contains(nextPoint.X * 1000 + nextPoint.Y))
                    {
                        visitedPoints.Add(nextPoint.X * 1000 + nextPoint.Y);
                        tan.Enqueue(nextPoint);
                    }
                }
                if (point.Y < map[0].Count - 2 && map[point.X][point.Y + 1].Content != '#')
                {
                    var nextPoint = new Neighboor(point.X, point.Y + 1, point.Distance + 1, point);
                    if (!visitedPoints.Contains(nextPoint.X * 1000 + nextPoint.Y))
                    {
                        visitedPoints.Add(nextPoint.X * 1000 + nextPoint.Y);
                        tan.Enqueue(nextPoint);
                    }
                }
                if (point.X < map.Count - 2 && map[point.X + 1][point.Y].Content != '#')
                {
                    var nextPoint = new Neighboor(point.X + 1, point.Y, point.Distance + 1, point);
                    if (!visitedPoints.Contains(nextPoint.X * 1000 + nextPoint.Y))
                    {
                        visitedPoints.Add(nextPoint.X * 1000 + nextPoint.Y);
                        tan.Enqueue(nextPoint);
                    }
                }

                return FindMinimumDistanceImpl(tan, visitedPoints);
            }
        }

        public override string ProcessSecond()
        {
            return 0.ToString();
        }

        private class Neighboor
        {
            public Neighboor(int x, int y, int d, Neighboor previous)
            {
                X = x;
                Y = y;
                Distance = d;
                Previous = previous;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Distance { get; set; }
            public Neighboor Previous { get; set; }
        }

        private class Point
        {
            public Point(int x, int y, bool isElf)
            {
                X = x;
                Y = y;
                IsElf = isElf;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public bool IsElf { get; set; }
        }

        private class Square
        {
            public Square(char content)
            {
                Content = content;
                switch (content)
                {
                    case 'E':
                        Unit = new Unit { IsElf = true };
                        break;
                    case 'G':
                        Unit = new Unit();
                        break;
                }
            }
            public char Content { get; set; }

            public Unit Unit { get; set; }
        }

        private class Unit
        {
            public int Life { get; set; } = 200;

            public bool IsElf { get; set; }
        }
    }
}