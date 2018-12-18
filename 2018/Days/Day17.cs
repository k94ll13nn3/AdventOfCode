using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day17 : Day
    {
        public override string Title => "Reservoir Research";

        public override string ProcessFirst()
        {
            var lines = GetLinesAsStrings();
            var maxX = 0;
            var maxY = 0;
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var data = new List<(string f, int col, string dir, int start, int end)>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '=', ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
                data.Add((parts[0], int.Parse(parts[1]), parts[2], int.Parse(parts[3]), int.Parse(parts[4])));

                if (parts[0] == "x")
                {
                    maxX = Math.Max(maxX, int.Parse(parts[1]));
                    minX = Math.Min(minX, int.Parse(parts[1]));
                }
                if (parts[0] == "y")
                {
                    maxY = Math.Max(maxY, int.Parse(parts[1]));
                    minY = Math.Min(minY, int.Parse(parts[1]));
                }
                if (parts[2] == "x")
                {
                    maxX = Math.Max(maxX, int.Parse(parts[4]));
                    minX = Math.Min(minX, int.Parse(parts[3]));
                }
                if (parts[2] == "y")
                {
                    maxY = Math.Max(maxY, int.Parse(parts[4]));
                    minY = Math.Min(minY, int.Parse(parts[3]));
                }
            }

            maxX += 1;
            var map = new char[maxY + 1][];
            for (int i = 0; i < maxY + 1; i++)
            {
                map[i] = new char[maxX + 1];
            }

            map[0][500] = '+';
            foreach (var d in data)
            {
                if (d.f == "x")
                {
                    for (int i = d.start; i <= d.end; i++)
                    {
                        map[i][d.col] = '#';
                    }
                }

                if (d.f == "y")
                {
                    for (int i = d.start; i <= d.end; i++)
                    {
                        map[d.col][i] = '#';
                    }
                }
            }

            bool isNotInfinite = true;
            var points = new Stack<(int y, int x, bool goUp)>();
            points.Push((0, 500, false));
            while (isNotInfinite)
            {
                if (points.Count == 0)
                {
                    isNotInfinite = false;
                    continue;
                }

                var current = points.Pop();
                if (current.y >= maxY)
                {
                    continue;
                }

                if (map[current.y + 1][current.x] == '\0')
                {
                    map[current.y + 1][current.x] = '|';
                    points.Push((current.y + 1, current.x, false));
                }
                else if (map[current.y + 1][current.x] == '#' || map[current.y + 1][current.x] == '~')
                {
                    if (map[current.y][current.x] == '~')
                    {
                        continue;
                    }

                    if (!current.goUp && map[current.y + 1][current.x] == '~')
                    {
                        var found = 0;
                        var tt = current.x - 1;
                        while (tt >= 0 && map[current.y + 1][tt] != '|' && map[current.y + 1][tt] != '#')
                        {
                            tt--;
                        }

                        if (tt >= 0 && map[current.y][tt] == '\0')
                        {
                            found++;
                        }

                        tt = current.x + 1;
                        while (tt <= maxX && map[current.y + 1][tt] != '|' && map[current.y + 1][tt] != '#')
                        {
                            tt++;
                        }

                        if (tt <= maxX && map[current.y][tt] == '\0')
                        {
                            found++;
                        }

                        if (found >= 2)
                        {
                            continue;
                        }
                    }

                    var doNotEnqueueNext = false;
                    var j = current.x;
                    while (j >= 0 && (map[current.y][j] == '\0' || map[current.y][j] == '|'))
                    {
                        if (map[current.y + 1][j] == '\0' || map[current.y + 1][j] == '|')
                        {
                            map[current.y][j] = '|';
                            points.Push((current.y, j, false));
                            doNotEnqueueNext = true;
                            for (int i2 = j; i2 <= current.x; i2++)
                            {
                                map[current.y][i2] = '|';
                            }
                            break;
                        }

                        map[current.y][j] = '~';
                        j--;
                    }

                    var stoppedJ = map[current.y][j] == '#' ? j + 1 : j;
                    j = current.x + 1;
                    while (j <= maxX && (map[current.y][j] == '\0' || map[current.y][j] == '|'))
                    {
                        if (map[current.y + 1][j] == '\0' || map[current.y + 1][j] == '|')
                        {
                            map[current.y][j] = '|';
                            points.Push((current.y, j, false));
                            doNotEnqueueNext = true;
                            for (int i2 = stoppedJ; i2 <= j; i2++)
                            {
                                map[current.y][i2] = '|';
                            }
                            break;
                        }

                        map[current.y][j] = doNotEnqueueNext ? '|' : '~';
                        j++;
                    }

                    if (!doNotEnqueueNext && current.y > 0)
                    {
                        points.Push((current.y - 1, current.x, true));
                    }
                }
            }

            var count = 0;
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = 0; j <= maxX; j++)
                {
                    count += map[i][j] == '~' || map[i][j] == '|' ? 1 : 0;
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            var lines = GetLinesAsStrings();
            var maxX = 0;
            var maxY = 0;
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var data = new List<(string f, int col, string dir, int start, int end)>();
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { '=', ',', ' ', '.' }, StringSplitOptions.RemoveEmptyEntries);
                data.Add((parts[0], int.Parse(parts[1]), parts[2], int.Parse(parts[3]), int.Parse(parts[4])));

                if (parts[0] == "x")
                {
                    maxX = Math.Max(maxX, int.Parse(parts[1]));
                    minX = Math.Min(minX, int.Parse(parts[1]));
                }
                if (parts[0] == "y")
                {
                    maxY = Math.Max(maxY, int.Parse(parts[1]));
                    minY = Math.Min(minY, int.Parse(parts[1]));
                }
                if (parts[2] == "x")
                {
                    maxX = Math.Max(maxX, int.Parse(parts[4]));
                    minX = Math.Min(minX, int.Parse(parts[3]));
                }
                if (parts[2] == "y")
                {
                    maxY = Math.Max(maxY, int.Parse(parts[4]));
                    minY = Math.Min(minY, int.Parse(parts[3]));
                }
            }

            maxX += 1;
            var map = new char[maxY + 1][];
            for (int i = 0; i < maxY + 1; i++)
            {
                map[i] = new char[maxX + 1];
            }

            map[0][500] = '+';
            foreach (var d in data)
            {
                if (d.f == "x")
                {
                    for (int i = d.start; i <= d.end; i++)
                    {
                        map[i][d.col] = '#';
                    }
                }

                if (d.f == "y")
                {
                    for (int i = d.start; i <= d.end; i++)
                    {
                        map[d.col][i] = '#';
                    }
                }
            }

            bool isNotInfinite = true;
            var points = new Stack<(int y, int x, bool goUp)>();
            points.Push((0, 500, false));
            while (isNotInfinite)
            {
                if (points.Count == 0)
                {
                    isNotInfinite = false;
                    continue;
                }

                var current = points.Pop();
                if (current.y >= maxY)
                {
                    continue;
                }

                if (map[current.y + 1][current.x] == '\0')
                {
                    map[current.y + 1][current.x] = '|';
                    points.Push((current.y + 1, current.x, false));
                }
                else if (map[current.y + 1][current.x] == '#' || map[current.y + 1][current.x] == '~')
                {
                    if (map[current.y][current.x] == '~')
                    {
                        continue;
                    }

                    if (!current.goUp && map[current.y + 1][current.x] == '~')
                    {
                        var found = 0;
                        var tt = current.x - 1;
                        while (tt >= 0 && map[current.y + 1][tt] != '|' && map[current.y + 1][tt] != '#')
                        {
                            tt--;
                        }

                        if (tt >= 0 && map[current.y][tt] == '\0')
                        {
                            found++;
                        }

                        tt = current.x + 1;
                        while (tt <= maxX && map[current.y + 1][tt] != '|' && map[current.y + 1][tt] != '#')
                        {
                            tt++;
                        }

                        if (tt <= maxX && map[current.y][tt] == '\0')
                        {
                            found++;
                        }

                        if (found >= 2)
                        {
                            continue;
                        }
                    }

                    var doNotEnqueueNext = false;
                    var j = current.x;
                    while (j >= 0 && (map[current.y][j] == '\0' || map[current.y][j] == '|'))
                    {
                        if (map[current.y + 1][j] == '\0' || map[current.y + 1][j] == '|')
                        {
                            map[current.y][j] = '|';
                            points.Push((current.y, j, false));
                            doNotEnqueueNext = true;
                            for (int i2 = j; i2 <= current.x; i2++)
                            {
                                map[current.y][i2] = '|';
                            }
                            break;
                        }

                        map[current.y][j] = '~';
                        j--;
                    }

                    var stoppedJ = map[current.y][j] == '#' ? j + 1 : j;
                    j = current.x + 1;
                    while (j <= maxX && (map[current.y][j] == '\0' || map[current.y][j] == '|'))
                    {
                        if (map[current.y + 1][j] == '\0' || map[current.y + 1][j] == '|')
                        {
                            map[current.y][j] = '|';
                            points.Push((current.y, j, false));
                            doNotEnqueueNext = true;
                            for (int i2 = stoppedJ; i2 <= j; i2++)
                            {
                                map[current.y][i2] = '|';
                            }
                            break;
                        }

                        map[current.y][j] = doNotEnqueueNext ? '|' : '~';
                        j++;
                    }

                    if (!doNotEnqueueNext && current.y > 0)
                    {
                        points.Push((current.y - 1, current.x, true));
                    }
                }
            }

            var count = 0;
            for (int i = minY; i <= maxY; i++)
            {
                for (int j = 0; j <= maxX; j++)
                {
                    count += map[i][j] == '~' ? 1 : 0;
                }
            }

            return count.ToString();
        }
    }
}