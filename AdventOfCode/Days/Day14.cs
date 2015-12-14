// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day14 : Day
    {
        private readonly List<Tuple<int, int, int>> competitors;

        private readonly int time = 2503;

        public Day14()
        {
            this.competitors = this.Lines.Select(l => l.Split(' ')).Select(t => Tuple.Create(int.Parse(t[3]), int.Parse(t[6]), int.Parse(t[13]))).ToList();
        }

        public override object ProcessFirst()
        {
            return FindLeaders(this.time, this.competitors)[0].Item1;
        }

        public override object ProcessSecond()
        {
            var points = new int[this.competitors.Count];

            Enumerable.Range(1, this.time).ToList().ForEach(s => FindLeaders(s, this.competitors).ForEach(tuple => points[tuple.Item2]++));

            return points.Max();
        }

        private static int ComputeDistanceTraveled(Tuple<int, int, int> tuple, int time)
        {
            var loopTime = tuple.Item2 + tuple.Item3;
            return (tuple.Item1 * tuple.Item2 * (time / loopTime)) + (Math.Min(tuple.Item2, time % loopTime) * tuple.Item1);
        }

        private static List<Tuple<int, int>> FindLeaders(int time, IEnumerable<Tuple<int, int, int>> competitors)
        {
            var results = competitors.Select(t => ComputeDistanceTraveled(t, time)).ToList();
            var returnValue = results.Select((v, i) => v == results.Max() ? Tuple.Create(v, i) : null).Where(t => t != null).ToList();

            return returnValue;
        }
    }
}