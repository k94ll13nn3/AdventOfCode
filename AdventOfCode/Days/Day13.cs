// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day13 : Day
    {
        public override object ProcessFirst()
        {
            var relations = ExtractRelations(this.Lines);
            var names = relations.Select(t => t.Key.Item1).Distinct();

            return ComputeHappiness(relations, names);
        }

        public override object ProcessSecond()
        {
            var relations = ExtractRelations(this.Lines);
            var names = relations.Select(t => t.Key.Item1).Distinct().ToList();

            foreach (var name in names)
            {
                relations.Add(Tuple.Create("k94ll13nn3", name), 0);
                relations.Add(Tuple.Create(name, "k94ll13nn3"), 0);
            }

            names.Add("k94ll13nn3");

            return ComputeHappiness(relations, names);
        }

        private static int ComputeHappiness(Dictionary<Tuple<string, string>, int> relations, IEnumerable<string> names)
        {
            var sum = int.MinValue;
            var permutations = GetPermutations(names, names.Count());

            foreach (var permutation in permutations)
            {
                var modified = permutation.ToList();
                modified.Add(modified[0]);
                sum = Math.Max(ComputeHappinessOfPermutation(modified, relations), sum);
            }

            return sum;
        }

        private static int ComputeHappinessOfPermutation(IList<string> permutation, Dictionary<Tuple<string, string>, int> relations)
        {
            var happiness = 0;

            for (int i = 0; i < permutation.Count - 1; i++)
            {
                happiness += relations[Tuple.Create(permutation[i], permutation[i + 1])];
                happiness += relations[Tuple.Create(permutation[i + 1], permutation[i])];
            }

            return happiness;
        }

        private static Dictionary<Tuple<string, string>, int> ExtractRelations(IEnumerable<string> lines)
        {
            var relations = new Dictionary<Tuple<string, string>, int>();

            foreach (var line in lines)
            {
                var tmp = TrimLine(line).Split(' ');
                relations.Add(Tuple.Create(tmp[0], tmp[2]), int.Parse(tmp[1]));
            }

            return relations;
        }

        private static IEnumerable<IEnumerable<string>> GetPermutations(IEnumerable<string> list, int length)
        {
            return length == 1
                ? list.Select(t => new[] { t })
                : GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new[] { t2 }).ToArray());
        }

        private static string TrimLine(string line)
        {
            return line.Replace(" happiness units by sitting next to", string.Empty).Replace(".", string.Empty).Replace("would gain ", string.Empty).Replace("would lose ", "-");
        }
    }
}