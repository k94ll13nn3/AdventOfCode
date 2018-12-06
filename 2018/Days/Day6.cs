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

            var (xMin, xMax, yMin, yMax) = (points.Select(x => x.x).Min(),points.Select(x => x.x).Max(), points.Select(x => x.y).Min(), points.Select(x => x.y).Max());
            var dico = new Dictionary<int, int>();

            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    var distances = points.Select((x, ind) => (d: Distance((x.x, x.y), (i, j)), ind: ind)).OrderBy(x => x.d);
                    if (distances.First().d != distances.Skip(1).First().d)
                    {
                        var t = distances.First().ind;
                        if (dico.ContainsKey(t))
                        {
                            dico[t]++;
                        }
                        else
                        {
                            dico[t] = 1;
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

            var (xMin, xMax, yMin, yMax) = (points.Select(x => x.x).Min(),points.Select(x => x.x).Max(), points.Select(x => x.y).Min(), points.Select(x => x.y).Max());
            var count = 0;
            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    var distanceSum = points.Select(x => Distance((x.x, x.y), (i, j))).Sum();
                    if (distanceSum < 10000)
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }

        private int Distance((int x, int y) pointA, (int x, int y) pointB) => Math.Abs(pointA.x - pointB.x) + Math.Abs(pointA.y - pointB.y);
    }
}