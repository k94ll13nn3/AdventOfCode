// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day9 : Day
    {
        public override object ProcessFirst()
        {
            return this.FindDistance(Math.Min, int.MaxValue);
        }

        public override object ProcessSecond()
        {
            return this.FindDistance(Math.Max, 0);
        }

        private static int ComputeDistance(Dictionary<Tuple<string, string>, int> distances, IList<string> r)
        {
            var count = 0;

            for (int i = 0; i < r.Count() - 1; i++)
            {
                count += distances[Tuple.Create(r[i], r[i + 1])];
            }

            return count;
        }

        private static IEnumerable<IEnumerable<string>> GetPermutations(IEnumerable<string> list, int length)
        {
            return length == 1
                ? list.Select(t => new[] { t })
                : GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
        }

        private object FindDistance(Func<int, int, int> function, int initValue)
        {
            var distances = new Dictionary<Tuple<string, string>, int>();

            foreach (var line in this.Lines)
            {
                var s = line.Split(' ');

                distances[Tuple.Create(s[0], s[2])] = distances[Tuple.Create(s[2], s[0])] = int.Parse(s[4]);
            }

            var cities = distances.Select(k => k.Key.Item1).Distinct();

            var permutations = GetPermutations(cities, cities.Count());
            var m = initValue;

            foreach (var permutation in permutations)
            {
                m = function(m, ComputeDistance(distances, permutation.ToList()));
            }

            return m;
        }
    }
}