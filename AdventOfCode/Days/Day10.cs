// stylecop.header

using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day10 : Day
    {
        public override object ProcessFirst()
        {
            return this.ComputeLookAndSay(40);
        }

        public override object ProcessSecond()
        {
            return this.ComputeLookAndSay(50);
        }

        private int ComputeLookAndSay(int loops)
        {
            const string pattern = @"((.)\2*)+";
            var result = this.Content;

            for (int i = 0; i < loops; i++)
            {
                var captureCollection = Regex.Match(result, pattern).Groups[1].Captures.Cast<Capture>();
                result = string.Join(string.Empty, captureCollection.Select(c => $"{c.Length}{c.Value[0]}"));
            }

            return result.Length;
        }
    }
}