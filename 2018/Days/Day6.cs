using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day6 : Day
    {
        public override string Title => "Chronal Coordinates";

        public override string ProcessFirst()
        {
            var points = GetLinesAsStrings()
                .Select(x => x.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select((x, i) => (ind: i, x: int.Parse(x[0]), y: int.Parse(x[1])))
                .ToList();

            var (xMin, xMax, yMin, yMax) = (points.Select(x => x.x).Min(), points.Select(x => x.x).Max(), points.Select(x => x.y).Min(), points.Select(x => x.y).Max());
            var dico = new Dictionary<int, int>();

            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    int index = -1;
                    int minimum = int.MaxValue;
                    int minimumSeen = int.MaxValue;
                    for (int k = 0; k < points.Count; k++)
                    {
                        var d = Distance(points[k].x, points[k].y, i, j);
                        if (d == minimum)
                        {
                            index = -1;
                            minimumSeen = minimum;
                        }
                        else if (d < minimum)
                        {
                            minimum = d;
                            index = k;
                        }
                    }

                    if (index != -1 && minimum != minimumSeen)
                    {
                        if (dico.ContainsKey(index))
                        {
                            dico[index]++;
                        }
                        else
                        {
                            dico[index] = 1;
                        }
                    }
                }
            }

            dico.Remove(points.OrderBy(x => x.x).ThenBy(x => x.y).First().ind);
            dico.Remove(points.OrderByDescending(x => x.x).ThenBy(x => x.y).First().ind);
            dico.Remove(points.OrderBy(x => x.x).ThenByDescending(x => x.y).First().ind);
            dico.Remove(points.OrderByDescending(x => x.x).ThenByDescending(x => x.y).First().ind);

            return dico.Values.Max().ToString();
        }

        public override string ProcessSecond()
        {
            var points = GetLinesAsStrings()
               .Select(x => x.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
               .Select(x => (x: int.Parse(x[0]), y: int.Parse(x[1])))
               .ToList();

            var (xMin, xMax, yMin, yMax) = (points.Select(x => x.x).Min(), points.Select(x => x.x).Max(), points.Select(x => x.y).Min(), points.Select(x => x.y).Max());
            var count = 0;
            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    var sum = 0;
                    for (int k = 0; k < points.Count; k++)
                    {
                        sum += Distance(points[k].x, points[k].y, i, j);
                        if (sum >= 10000)
                        {
                            break;
                        }
                    }

                    if (sum < 10000)
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }

        private int Distance(int a, int b, int c, int d) => Math.Abs(a - c) + Math.Abs(b - d);
    }
}