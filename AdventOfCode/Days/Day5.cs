using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day5 : Day
    {
        public Day5()
            : base(5)
        {
        }

        public override object ProcessFirst()
        {
            return this.Lines.Count(IsNice);
        }

        public override object ProcessSecond()
        {
            return this.Lines.Count(IsNicer);
        }

        private static bool IsNice(string s)
        {
            const string vowels = "aeiou";
            var excluded = new string[] { "ab", "cd", "pq", "xy" };

            var splittedString = s.Select((c, i) => i == s.Length - 1 ? null : $"{s[i]}{s[i + 1]}").Where(c => c != null);

            foreach (var item in splittedString)
            {
                if (excluded.Contains(item))
                {
                    return false;
                }
            }

            return (s.Count(vowels.Contains) >= 3) && splittedString.Any(c => c[0] == c[1]);
        }

        private static bool IsNicer(string s)
        {
            var splittedString = s.Select((c, i) => i == s.Length - 1 ? null : $"{s[i]}{s[i + 1]}").Where(c => c != null);
            var pairFound = false;

            foreach (var item in splittedString)
            {
                var index = s.IndexOf(item, System.StringComparison.Ordinal);

                // the current item is replaced by "!!"
                var miniString = s.Remove(index, 2);
                miniString = miniString.Insert(index, "!!");
                if (miniString.IndexOf(item, System.StringComparison.Ordinal) != -1)
                {
                    pairFound = true;
                    break;
                }
            }

            return pairFound && s.Select((c, i) => i >= s.Length - 2 ? false : s[i] == s[i + 2]).Any(c => c);
        }
    }
}