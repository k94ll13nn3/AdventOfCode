using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day10 : Day
    {
        public override string Title => "Adapter Array";

        public override string ProcessFirst()
        {
            var adapters = GetLines().Select(int.Parse).OrderBy(x => x).ToList();

            adapters.Insert(0, 0);
            adapters.Add(adapters[^1] + 3);

            int diff1 = 0;
            int diff3 = 0;
            for (int i = 0; i < adapters.Count - 1; i++)
            {
                if (adapters[i + 1] - adapters[i] == 1)
                {
                    diff1++;
                }
                else
                {
                    diff3++;
                }
            }

            return $"{diff1 * diff3}";
        }

        public override string ProcessSecond()
        {
            var adaptersTmp = GetLines().Select(int.Parse).OrderBy(x => x).ToList();

            adaptersTmp.Insert(0, 0);
            adaptersTmp.Add(adaptersTmp[^1] + 3);

            int[] adapters = adaptersTmp.ToArray();

            var groups = new List<(int start, int end)>();
            int lastI = 0;
            for (int i = 0; i < adapters.Length - 1; i++)
            {
                if (adapters[i + 1] - adapters[i] == 3)
                {
                    groups.Add((lastI, i));
                    lastI = i + 1;
                }
            }

            groups.Add((lastI, adapters.Length - 1));

            long count = 1;
            foreach ((int start, int end) in groups)
            {
                if (start != end)
                {
                    count *= FindPossibilities(adapters[start], adapters[(start + 1)..(end + 1)]);
                }
            }

            return $"{count}";
        }

        private int FindPossibilities(int start, int[] rest)
        {
            if (rest.Length <= 1)
            {
                return 1;
            }

            int c = 0;
            for (int i = 0; i < rest.Length; i++)
            {
                if (rest[i] <= start + 3)
                {
                    c += FindPossibilities(rest[i], rest[(i + 1)..]);
                }
                else
                {
                    break;
                }
            }
            return c;
        }
    }
}
