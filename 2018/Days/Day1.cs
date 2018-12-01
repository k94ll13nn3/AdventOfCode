using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day1 : Day
    {
        public override string Title => "Chronal Calibration";

        public override string ProcessFirst()
        {
            return GetLines().Select(x => int.Parse(x.Span)).Sum().ToString();
        }

        public override string ProcessSecond()
        {
            var frequencies = new HashSet<int>();
            var currentFrequency = 0;
            var modifications = GetLines().Select(x => int.Parse(x.Span)).ToList();
            var index = 0;
            while(!frequencies.Contains(currentFrequency))
            {
                frequencies.Add(currentFrequency);
                currentFrequency += modifications[index];
                index = (index + 1) % modifications.Count;
            }
            
            return currentFrequency.ToString();
        }
    }
}