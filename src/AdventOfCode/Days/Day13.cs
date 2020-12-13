using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day13 : Day
    {
        public override string Title => "Shuttle Search";

        public override string ProcessFirst()
        {
            var input = GetLines().ToList();
            int timestamp = int.Parse(input[0]);
            IEnumerable<int> busses = input[1].Split(',').Where(x => int.TryParse(x, out _)).Select(int.Parse);

            int minimum = int.MaxValue;
            int goodBus = 0;
            foreach (int bus in busses)
            {
                int waitTime = (int)(Math.Ceiling((double)timestamp / bus) * bus);
                if (waitTime < minimum)
                {
                    minimum = waitTime;
                    goodBus = bus;
                }
            }

            return $"{goodBus * (minimum - timestamp)}";
        }

        public override string ProcessSecond()
        {
            IList<(long val, int ind)> busses = GetLines()
                .ToList()[1]
                .Split(',')
                .Select((x, i) => long.TryParse(x, out long v) ? (val: v, ind: i) : (val: 0, ind: -1))
                .Where(x => x.val != 0)
                .ToList();

            long cycle = busses[0].val;
            int i = 1;
            long timestamp = 0;

            // For each bus, we search for the cycle with the busses before.
            // When a matching t is found, the next t will be at the precedent cycle * the bus value because all busses are primary
            // number so the cycle (which is the lcm) is just the multiplication of each bus.
            // At the end, the result is the last t minus the cycle to have the start.
            for (timestamp = busses[0].val; i < busses.Count; timestamp += cycle)
            {
                if ((timestamp + busses[i].ind) % busses[i].val == 0)
                {
                    cycle *= busses[i].val;
                    i++;
                }
            }

            return $"{timestamp - cycle}";
        }
    }
}
