// stylecop.header

using System;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
    internal class Day20 : Day
    {
        public Day20()
            : base("29000000")
        {
        }

        public override object ProcessFirst()
        {
            var start = 1;

            while (FindNumberOfPresents(start) < int.Parse(this.Content))
            {
                start++;
            }

            return start;
        }

        public override object ProcessSecond()
        {
            var start = 1;

            while (FindNumberOfPresentsForLazyElves(start) < int.Parse(this.Content))
            {
                start++;
            }

            return start;
        }

        private static IEnumerable<int> Factor(int number)
        {
            var max = (int)Math.Sqrt(number);
            for (int factor = 1; factor <= max; ++factor)
            {
                if (number % factor == 0)
                {
                    yield return factor;
                    if (factor != number / factor)
                    {
                        yield return number / factor;
                    }
                }
            }
        }

        private static int FindNumberOfPresentsForLazyElves(int house)
        {
            var count = 0;
            foreach (var factor in Factor(house))
            {
                if (factor * 50 >= house)
                {
                    count += factor * 11;
                }
            }

            return count;
        }

        private static int FindNumberOfPresents(int house)
        {
            var count = 0;
            foreach (var factor in Factor(house))
            {
                count += factor * 10;
            }

            return count;
        }
    }
}