using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day2 : Day
    {
        public override string Title => "Password Philosophy";

        public override string ProcessFirst()
        {
            int count = 0;
            foreach (string line in GetLines())
            {
                (string pattern, string password) = line.Split(':');
                (string lower, string upper, string letter) = pattern.Split(new[] { '-', ' ' });
                int letterCount = password.Count(c => c == letter[0]);
                if (letterCount >= int.Parse(lower) && letterCount <= int.Parse(upper))
                {
                    count++;
                }
            }

            return $"{count}";
        }

        public override string ProcessSecond()
        {
            int count = 0;
            foreach (string line in GetLines())
            {
                (string pattern, string password) = line.Split(':');
                (string lower, string upper, string letter) = pattern.Split(new[] { '-', ' ' });
                if (password.Trim()[int.Parse(lower) - 1] == letter[0] ^ password.Trim()[int.Parse(upper) - 1] == letter[0])
                {
                    count++;
                }
            }

            return $"{count}";
        }
    }
}
