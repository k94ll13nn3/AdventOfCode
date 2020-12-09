using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day9 : Day
    {
        public override string Title => "Encoding Error";

        public override string ProcessFirst()
        {
            long[] number = GetLines().Select(long.Parse).ToArray();
            const int preamble = 25;
            long error = 0;
            for (int i = preamble; i < number.Length; i++)
            {
                if (!number[(i - preamble)..i].Any(x => number[(i - preamble)..i].Contains(number[i] - x)))
                {
                    error = number[i];
                    break;
                }
            }

            return $"{error}";
        }

        public override string ProcessSecond()
        {
            long[] number = GetLines().Select(long.Parse).ToArray();
            const int preamble = 25;
            long error = 0;
            for (int i = preamble; i < number.Length; i++)
            {
                if (!number[(i - preamble)..i].Any(x => number[(i - preamble)..i].Contains(number[i] - x)))
                {
                    error = number[i];
                    break;
                }
            }

            int lower = 0;
            int upper = 1;
            long sum = number[lower..upper].Sum();
            while (sum != error)
            {
                if (sum < error)
                {
                    upper++;
                }
                else
                {
                    lower++;
                }

                sum = number[lower..upper].Sum();
            }

            return $"{number[lower..upper].Min() + number[lower..upper].Max()}";
        }
    }
}
