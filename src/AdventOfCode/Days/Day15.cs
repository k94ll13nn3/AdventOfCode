using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day15 : Day
    {
        public override string Title => "Rambunctious Recitation";

        public override string ProcessFirst()
        {
            return $"{Compute(2020)}";
        }

        public override string ProcessSecond()
        {
            return $"{Compute(30000000)}";
        }

        private int Compute(int steps)
        {
            var numbers = Array.ConvertAll(GetContent().Split(','), int.Parse).ToList();
            int last = numbers[^1];
            var d = new Dictionary<int, (int c, int ia, int ib)>();
            for (int i = 0; i < numbers.Count; i++)
            {
                d[numbers[i]] = (1, -1, i);
            }

            for (int i = numbers.Count; i < steps; i++)
            {
                if (d[last].c == 1)
                {
                    d[0] = (d[0].c + 1, d[0].ib, i);
                    last = 0;
                }
                else
                {
                    int ind = d[last].ib - d[last].ia;
                    if (!d.ContainsKey(ind))
                    {
                        d[ind] = (0, -1, -1);
                    }
                    d[ind] = (d[ind].c + 1, d[ind].ib, i);
                    last = ind;
                }
            }

            return last;
        }
    }
}
