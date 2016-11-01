// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day24 : Day
    {
        private static long numberOfPresentsInFirstGroup = int.MaxValue;

        private static long quantumEntanglement = long.MaxValue;

        private static List<long> weights;

        public Day24()
        {
            weights = this.Lines.Select(long.Parse).OrderByDescending(i => i).ToList();
        }

        public override object ProcessFirst()
        {
            var goal = weights.Sum() / 3;

            Process(weights, goal, new List<long>());

            return quantumEntanglement;
        }

        public override object ProcessSecond()
        {
            var goal = weights.Sum() / 4;

            Process(weights, goal, new List<long>());

            return quantumEntanglement;
        }

        // It (wrongly?) assume that if you find a group matching the goal, the rest can be divided in groups too.
        // The smallest group has a size between 1 and Math.Ceiling(weights.Count / 3.0)
        private static void Process(List<long> numbers, long goal, List<long> currentGroup)
        {
            var possibilities = numbers.Where(i => i <= goal).ToList();

            var quantumCurrentGroup = currentGroup.Count == 0 ? 0 : currentGroup.Aggregate((a, b) => a * b);

            if (currentGroup.Count > Math.Min(numberOfPresentsInFirstGroup, Math.Ceiling(weights.Count / 3.0)) || quantumCurrentGroup > quantumEntanglement)
            {
                return;
            }

            for (int i = 0; i < possibilities.Count; i++)
            {
                if (possibilities[i] == goal)
                {
                    var group = weights.Where(j => !numbers.Contains(j)).ToList();
                    group.Add(possibilities[i]);
                    var cc = group.Aggregate((a, b) => a * b);

                    if (numberOfPresentsInFirstGroup >= group.Count && cc < quantumEntanglement)
                    {
                        numberOfPresentsInFirstGroup = group.Count;
                        quantumEntanglement = cc;

                        // This line allows us to see the current minimum quantumEntanglement
                        // this might be the solution if it stays long enough
                        // (very bad resolution method)
                        Console.WriteLine(quantumEntanglement);
                    }

                    return;
                }

                var rest = numbers.Where(j => j != possibilities[i]).ToList();

                var newGroup = currentGroup.Select(c => c).ToList();
                newGroup.Add(possibilities[i]);
                Process(rest, goal - possibilities[i], newGroup);
            }

            return;
        }
    }
}