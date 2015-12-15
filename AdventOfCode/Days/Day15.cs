// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day15 : Day
    {
        private readonly IList<int[]> ingredients;

        public Day15()
        {
            this.ingredients = this.Lines
                    .Select(line => line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries))
                    .Select(t => new[] { int.Parse(t[2]), int.Parse(t[4]), int.Parse(t[6]), int.Parse(t[8]), int.Parse(t[10]) })
                    .ToList();
        }

        public override object ProcessFirst()
        {
            return this.ComputeTotal(i => false);
        }

        public override object ProcessSecond()
        {
            return this.ComputeTotal(i => i != 500);
        }

        private static IEnumerable<IList<int>> ListAllPossibilities(int number, int max)
        {
            var res = new List<IList<int>>();

            if (number == 1)
            {
                res.Add(new List<int> { max });
            }
            else
            {
                for (int i = 0; i <= max; i++)
                {
                    foreach (var item in ListAllPossibilities(number - 1, max - i))
                    {
                        item.Insert(0, i);
                        res.Add(item);
                    }
                }
            }

            return res;
        }

        private int ComputeTotal(Predicate<int> predicate)
        {
            var possibilities = ListAllPossibilities(this.ingredients.Count, 100);

            var total = int.MinValue;
            foreach (var possibility in possibilities)
            {
                var totals = new int[] { 0, 0, 0, 0, 0 };

                for (int i = 0; i < this.ingredients.Count; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        totals[j] += this.ingredients[i][j] * possibility[i];
                    }
                }

                if (predicate(totals[4]))
                {
                    continue;
                }

                for (int j = 0; j < 4; j++)
                {
                    totals[j] = totals[j] < 0 ? 0 : totals[j];
                }

                total = Math.Max(total, totals[0] * totals[1] * totals[2] * totals[3]);
            }

            return total;
        }
    }
}