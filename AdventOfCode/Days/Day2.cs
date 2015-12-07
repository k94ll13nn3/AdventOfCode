using System;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day2 : Day
    {
        public Day2()
            : base(2)
        {
        }

        public override object ProcessFirst()
        {
            var count = 0;
            foreach (var line in this.Lines)
            {
                count += ComputeLine1(line);
            }

            return count;
        }

        public override object ProcessSecond()
        {
            var count = 0;
            foreach (var line in this.Lines)
            {
                count += ComputeLine2(line);
            }

            return count;
        }

        private static int ComputeLine1(string line)
        {
            var dim = line.Split('x').Select(i => int.Parse(i)).ToList();

            return (2 * dim[0] * dim[1]) + (2 * dim[1] * dim[2]) + (2 * dim[2] * dim[0]) + Math.Min(dim[0] * dim[1], Math.Min(dim[1] * dim[2], dim[2] * dim[0]));
        }

        private static int ComputeLine2(string line)
        {
            var dim = line.Split('x').Select(i => int.Parse(i)).ToList();
            var count = dim[0] * dim[1] * dim[2];

            dim.Remove(dim.Max());

            count += (2 * dim[0]) + (2 * dim[1]);

            return count;
        }
    }
}