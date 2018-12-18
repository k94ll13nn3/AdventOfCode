using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day18 : Day
    {
        public override string Title => "Settlers of The North Pole";

        public override string ProcessFirst()
        {
            return Compute(10);
        }
        public override string ProcessSecond()
        {
            return Compute(1000000000);
        }
        public string Compute(int maxMinutes)
        {
            var mapTmp = GetLinesAsStrings().Select(x => ("." + x + ".").ToArray());
            var l = mapTmp.First().Count();
            var map = mapTmp.Prepend(new string('.', l).ToCharArray()).Append(new string('.', l).ToCharArray()).ToArray();
            var minute = 0;
            var counts = new Dictionary<ulong, char[][]>();
            ulong start = 0;
            var minuteStart = 0;
            while (minute < maxMinutes)
            {
                var newMap = map.Select(x => x.ToArray()).ToArray();

                for (int i = 1; i < map.Length - 1; i++)
                {
                    for (int j = 1; j < map[0].Length - 1; j++)
                    {
                        switch (map[i][j])
                        {
                            case '.':
                                var count = 0;
                                for (int x = i - 1; x <= i + 1; x++)
                                {
                                    for (int y = j - 1; y <= j + 1; y++)
                                    {
                                        if (x == i && y == j)
                                        {
                                            continue;
                                        }

                                        if (map[x][y] == '|')
                                        {
                                            count++;
                                        }
                                    }
                                }
                                if (count >= 3)
                                {
                                    newMap[i][j] = '|';
                                }
                                break;
                            case '|':
                                var count2 = 0;
                                for (int x = i - 1; x <= i + 1; x++)
                                {
                                    for (int y = j - 1; y <= j + 1; y++)
                                    {
                                        if (x == i && y == j)
                                        {
                                            continue;
                                        }

                                        if (map[x][y] == '#')
                                        {
                                            count2++;
                                        }
                                    }
                                }
                                if (count2 >= 3)
                                {
                                    newMap[i][j] = '#';
                                }
                                break;
                            case '#':
                                var count3 = 0;
                                var count4 = 0;
                                for (int x = i - 1; x <= i + 1; x++)
                                {
                                    for (int y = j - 1; y <= j + 1; y++)
                                    {
                                        if (x == i && y == j)
                                        {
                                            continue;
                                        }

                                        if (map[x][y] == '#')
                                        {
                                            count3++;
                                        }
                                        if (map[x][y] == '|')
                                        {
                                            count4++;
                                        }
                                    }
                                }
                                if (count3 < 1 || count4 < 1)
                                {
                                    newMap[i][j] = '.';
                                }
                                break;
                        }
                    }
                }

                map = newMap;
                minute++;

                ulong hash = 0;
                for (int i = 1; i < map.Length - 1; i++)
                {
                    for (int j = 1; j < map[0].Length - 1; j++)
                    {
                        hash = hash * 31 + map[i][j];
                    }
                }

                if (start != 0 && hash != start)
                {
                    counts.Add(hash, map);
                }
                else if (start != 0 && hash == start)
                {
                    break;
                }
                else if (counts.ContainsKey(hash))
                {
                    start = hash;
                    minuteStart = minute;
                    counts.Clear();
                    counts.Add(hash, map);
                }
                else
                {
                    counts.Add(hash, map);
                }
            }

            char[][] res;
            if (start == 0)
            {
                res = counts.Last().Value;
            }
            else
            {
                res = counts.Skip((maxMinutes - minuteStart) % counts.Count).First().Value;
            }

            var wood = 0;
            var lumb = 0;
            for (int i = 1; i < res.Length - 1; i++)
            {
                for (int j = 1; j < res.Length - 1; j++)
                {
                    if (res[i][j] == '|')
                    {
                        wood++;
                    }
                    if (res[i][j] == '#')
                    {
                        lumb++;
                    }
                }
            }
            return (wood * lumb).ToString();
        }
    }
}