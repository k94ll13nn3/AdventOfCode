using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day6 : Day
    {
        public override string Title => "Binary Boarding";

        public override string ProcessFirst()
        {
            string[] groups = GetContent().Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            int count = 0;

            foreach (string group in groups)
            {
                count += group.Replace("\r\n", "").Distinct().Count();
            }

            return $"{count}";
        }

        public override string ProcessSecond()
        {
            string[] groups = GetContent().Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);
            int count = 0;

            foreach (string group in groups)
            {
                count += IntersectAll(group.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)).Count;
            }

            return $"{count}";
        }

        private static HashSet<T> IntersectAll<T>(IList<IEnumerable<T>> lists)
        {
            return lists.Skip(1).Aggregate(new HashSet<T>(lists[0]), (h, e) => { h.IntersectWith(e); return h; });
        }
    }
}
