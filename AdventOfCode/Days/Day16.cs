// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day16 : Day
    {
        private readonly List<Dictionary<string, int>> aunts;

        public Day16()
        {
            this.aunts = new List<Dictionary<string, int>>();

            foreach (string line in this.Lines)
            {
                var tokens = line.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
                this.aunts.Add(new Dictionary<string, int>
                    {
                        { tokens[2], int.Parse(tokens[3]) },
                        { tokens[4], int.Parse(tokens[5]) },
                        { tokens[6], int.Parse(tokens[7]) }
                    });
            }
        }

        public override object ProcessFirst()
        {
            var mfcsam = new Dictionary<string, Func<int, bool>>
                {
                    { "children", new Func<int, bool>(a => a == 3) },
                    { "cats", new Func<int, bool>(a => a == 7) },
                    { "samoyeds", new Func<int, bool>(a => a == 2) },
                    { "pomeranians", new Func<int, bool>(a => a == 3) },
                    { "akitas", new Func<int, bool>(a => a == 0) },
                    { "vizslas", new Func<int, bool>(a => a == 0) },
                    { "goldfish", new Func<int, bool>(a => a == 5) },
                    { "trees", new Func<int, bool>(a => a == 3) },
                    { "cars", new Func<int, bool>(a => a == 2) },
                    { "perfumes", new Func<int, bool>(a => a == 1) }
                };
            return ComputeAunt(this.aunts, mfcsam);
        }

        public override object ProcessSecond()
        {
            var mfcsam = new Dictionary<string, Func<int, bool>>
                {
                    { "children", new Func<int, bool>(a => a == 3) },
                    { "cats", new Func<int, bool>(a => a > 7) },
                    { "samoyeds", new Func<int, bool>(a => a == 2) },
                    { "pomeranians", new Func<int, bool>(a => a < 3) },
                    { "akitas", new Func<int, bool>(a => a == 0) },
                    { "vizslas", new Func<int, bool>(a => a == 0) },
                    { "goldfish", new Func<int, bool>(a => a < 5) },
                    { "trees", new Func<int, bool>(a => a > 3) },
                    { "cars", new Func<int, bool>(a => a == 2) },
                    { "perfumes", new Func<int, bool>(a => a == 1) }
                };
            return ComputeAunt(this.aunts, mfcsam);
        }

        private static int ComputeAunt(List<Dictionary<string, int>> aunts, Dictionary<string, Func<int, bool>> mfcsam)
        {
            for (int i = 0; i < aunts.Count; i++)
            {
                if (aunts[i].Count(x => mfcsam[x.Key].Invoke(x.Value)) == 3)
                {
                    return i + 1;
                }
            }

            return -1;
        }
    }
}