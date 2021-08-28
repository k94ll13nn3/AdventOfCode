using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day23 : Day
    {
        public override string Title => "Crab Cups";

        public override string ProcessFirst()
        {
            var input = GetContent().Replace(Environment.NewLine, "").Select(c => int.Parse(c.ToString())).ToArray();
            for (int i = 0; i < 100; i++)
            {
                input = input[1..].Concat(new[] { input[0] }).ToArray();
                int destination = input[^1];
                int destinationIndex = FindDest(input[3..].ToList(), destination, input.Max()) + 4;

                input = input[3..destinationIndex].Concat(input[0..3].Concat(input[destinationIndex..])).ToArray();
            }

            var indexOfOne = input.ToList().IndexOf(1) + 1;
            return new string(input[indexOfOne..].Concat(input[0..(indexOfOne - 1)]).Select(x => x.ToString()[0]).ToArray());
        }

        public override string ProcessSecond()
        {
            var input = GetContent().Replace(Environment.NewLine, "").Select(c => int.Parse(c.ToString())).Concat(Enumerable.Range(10, 1000000-10)).ToArray();
            for (int i = 0; i < 100; i++)
            {
                input = input[1..].Concat(new[] { input[0] }).ToArray();
                int destination = input[^1];
                int destinationIndex = FindDest(input[3..].ToList(), destination, input.Max()) + 4;

                input = input[3..destinationIndex].Concat(input[0..3].Concat(input[destinationIndex..])).ToArray();
            }

            var indexOfOne = input.ToList().IndexOf(1) + 1;
            return "-";
        }

        private static int FindDest(List<int> list, int currentCup, int maximum)
        {
            int cupToSearch = currentCup - 1;
            while (cupToSearch >= 0)
            {
                int ind = list.IndexOf(cupToSearch);
                if (ind != -1)
                {
                    return ind;
                }
                cupToSearch--;

                if (cupToSearch <= 0)
                {
                    cupToSearch = maximum;
                }
            }

            return -1;
        }
    }
}
