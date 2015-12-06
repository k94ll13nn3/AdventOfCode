﻿using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day1 : Day
    {
        public Day1() : base(1)
        {
        }

        public override object ProcessFirst()
        {
            var count = Content.Count(c => c == '(') - Content.Count(c => c == ')');

            return count;
        }

        public override object ProcessSecond()
        {
            var count = 0;

            for (int i = 0; i < Content.Length; i++)
            {
                count += Content[i] == '(' ? 1 : -1;
                if (count == -1)
                {
                    return i + 1;
                }
            }

            return 42;
        }
    }
}