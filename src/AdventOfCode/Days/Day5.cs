using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day5 : Day
    {
        public override string Title => "Binary Boarding";

        public override string ProcessFirst()
        {
            return $"{GetLines().Max(x => Convert.ToInt32(x.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'), 2))}";
        }

        public override string ProcessSecond()
        {
            HashSet<int> seats = GetLines().Select(x => Convert.ToInt32(x.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'), 2)).ToHashSet();

            return $"{seats.Where(x => !seats.Contains(x - 1) || !seats.Contains(x + 1)).OrderBy(x => x).Skip(1).First() + 1}";
        }
    }
}
