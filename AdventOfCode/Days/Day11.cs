// stylecop.header

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day11 : Day
    {
        private static readonly Predicate<string> HasExcludedLetter = s => s.Any(c => c == 'i' || c == 'o' || c == 'l');

        private static readonly Predicate<string> HasPair = s => Regex.Match(s, @"(.)\1.*(.)\2").Success;

        private static readonly Predicate<string> HasTriplet =
            s =>
            s.Select((c, i) => i < s.Length - 2 ? c == s[i + 1] - 1 && s[i + 1] == s[i + 2] - 1 : false).Any(c => c);

        public Day11()
            : base("hxbxwxba")
        {
        }

        public override object ProcessFirst()
        {
            var next = NextPassword(this.Content);
            while (!IsGoodPaswword(next))
            {
                next = NextPassword(next);
            }

            return next;
        }

        public override object ProcessSecond()
        {
            var next = NextPassword("hxbxxyzz");
            while (!IsGoodPaswword(next))
            {
                next = NextPassword(next);
            }

            return next;
        }

        private static bool IsGoodPaswword(string password)
        {
            return HasPair(password) && !HasExcludedLetter(password) && HasTriplet(password);
        }

        private static string NextPassword(string password)
        {
            var newPassword = password;

            if (HasExcludedLetter(newPassword))
            {
                var i = newPassword.IndexOfAny(new[] { 'i', 'o', 'l' });
                var c = (char)(newPassword[i] + 1);
                return newPassword.Remove(i) + c + new string('a', newPassword.Length - i - 1);
            }

            if (newPassword.Length == 1)
            {
                var c = (char)(newPassword[0] + 1);
                return c.ToString();
            }

            if (newPassword[newPassword.Length - 1] != 'z')
            {
                var c = (char)(newPassword[newPassword.Length - 1] + 1);
                return newPassword.Remove(newPassword.Length - 1) + c;
            }

            return NextPassword(newPassword.Remove(newPassword.Length - 1)) + 'a';
        }
    }
}