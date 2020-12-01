using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day1 : Day
    {
        public override string Title => "Report Repair";

        public override string ProcessFirst()
        {
            var inputs = GetLines().Select(int.Parse).ToHashSet();
            int result = Find(2020, inputs);
            return $"{result * (2020 - result)}";
        }

        public override string ProcessSecond()
        {
            var inputs = GetLines().Select(int.Parse).ToHashSet();
            int result = inputs.First(x => inputs.Contains(Find(2020 - x, inputs)));
            int k = Find(2020 - result, inputs);
            return $"{result * k * (2020 - k - result)}";
        }

        private static int Find(int value, HashSet<int> inputs)
        {
            return inputs.FirstOrDefault(x => inputs.Contains(value - x));
        }
    }
}
