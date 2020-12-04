using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day4 : Day
    {
        public override string Title => "Passport Processing";

        public override string ProcessFirst()
        {
            string content = GetContent()
                .Replace("\r\n\r\n", "_____")
                .Replace("\r\n", " ")
                .Replace("_____", "\r\n");

            IEnumerable<Dictionary<string, string>> passwords = content
                .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToDictionary(y => y.Split(':')[0], y => y.Split(':')[1]));

            return $"{passwords.Count(x => x.Keys.Count == 8 || (x.Keys.Count == 7 && !x.ContainsKey("cid")))}";
        }

        public override string ProcessSecond()
        {
            string content = GetContent()
                .Replace("\r\n\r\n", "_____")
                .Replace("\r\n", " ")
                .Replace("_____", "\r\n");

            IEnumerable<Dictionary<string, string>> passwords = content
                .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToDictionary(y => y.Split(':')[0], y => y.Split(':')[1]));

            int count = 0;
            foreach (Dictionary<string, string> password in passwords)
            {
                if (password.Keys.Count != 8 && (password.Keys.Count != 7 || password.ContainsKey("cid")))
                {
                    continue;
                }

                int valid = 0;
                if (ValidateInterval(password["byr"], 1920, 2002))
                {
                    valid++;
                }

                if (ValidateInterval(password["iyr"], 2010, 2020))
                {
                    valid++;
                }

                if (ValidateInterval(password["eyr"], 2020, 2030))
                {
                    valid++;
                }

                if (password["hgt"].Length == 5 && password["hgt"][3..] == "cm" && ValidateInterval(password["hgt"][..3], 150, 193))
                {
                    valid++;
                }

                if (password["hgt"].Length == 4 && password["hgt"][2..] == "in" && ValidateInterval(password["hgt"][..2], 59, 76))
                {
                    valid++;
                }

                if (password["hcl"][0] == '#' && password["hcl"].Length == 7 && password["hcl"][1..].All(c => c is (>= '0' and <= '9') or (>= 'a' and <= 'z')))
                {
                    valid++;
                }

                if (new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(password["ecl"]))
                {
                    valid++;
                }

                if (password["pid"].Length == 9 && password["pid"].All(c => c is >= '0' and <= '9'))
                {
                    valid++;
                }

                if (valid == 7)
                {
                    count++;
                }
            }

            return $"{count}";
        }

        private static bool ValidateInterval(string value, int min, int max)
        {
            return int.TryParse(value, out int parsedValue) && parsedValue >= min && parsedValue <= max;
        }
    }
}
