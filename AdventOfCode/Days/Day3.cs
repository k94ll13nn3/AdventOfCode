using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day3 : Day
    {
        public Day3() : base(3)
        {
        }

        public override object ProcessFirst()
        {
            return ComputeList(Content).Count();
        }

        public override object ProcessSecond()
        {
            var a = new string(Content.Where((c, i) => i % 2 == 0).ToArray());
            var b = new string(Content.Where((c, i) => i % 2 != 0).ToArray());

            var houses = new List<Tuple<int, int>>(ComputeList(a));
            houses.AddRange(ComputeList(b));

            return houses.Distinct().Count();
        }

        private static IEnumerable<Tuple<int, int>> ComputeList(string s)
        {
            var houses = new List<Tuple<int, int>>();
            var currentHouse = new Tuple<int, int>(0, 0);
            houses.Add(currentHouse);

            foreach (var c in s)
            {
                switch (c)
                {
                    case '^':
                        currentHouse = new Tuple<int, int>(currentHouse.Item1, currentHouse.Item2 + 1);
                        break;

                    case 'v':
                        currentHouse = new Tuple<int, int>(currentHouse.Item1, currentHouse.Item2 - 1);
                        break;

                    case '>':
                        currentHouse = new Tuple<int, int>(currentHouse.Item1 + 1, currentHouse.Item2);
                        break;

                    case '<':
                        currentHouse = new Tuple<int, int>(currentHouse.Item1 - 1, currentHouse.Item2);
                        break;
                }

                if (!houses.Contains(currentHouse))
                {
                    houses.Add(currentHouse);
                }
            }

            return houses;
        }
    }
}