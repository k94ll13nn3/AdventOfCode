using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day19 : Day
    {
        private static readonly Dictionary<int, string> Memory = new();
        public override string Title => "Monster Messages";

        public override string ProcessFirst()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var rules = input[0].Select(x => x.Split(':')).ToDictionary(x => int.Parse(x[0].Trim()), x => x[1].Trim());
            var regex = new Regex($"^{GetRule(rules, 0)}$", RegexOptions.Compiled);

            return $"{input[1].Count(x => regex.IsMatch(x))}";
        }

        public override string ProcessSecond()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var rules = input[0].Select(x => x.Split(':')).ToDictionary(x => int.Parse(x[0].Trim()), x => x[1].Trim());

            // New rules (no need to update the input, the code will take care of the changes):
            // [8] = "42 | 42 8"; // Match any number of 42
            // [11] = "42 31 | 42 11 31"; // Match x number of 42 then x number 31, same x for both.

            // Create a Regex with a balancing group definition of form ^(A)+(?<G>A)+(?<-G>B)+$
            // This will match at least one number of pattern A, then any number of pattern A stored in the group G,
            // then will try to match any pattern B, but for each pattern B, a pattern A must have been found by the group G.
            // So AAB will match because A is matched, the A is captured, then B can be matched because one A was captured.
            // So AB will not match because A is matched, then B cannot be matched because no A was captured.
            // So AABB will not match because A is matched, the A is captured, then B can be matched because one A was captured but
            // the last B cannot be matched because only one A was captured.
            string pattern = $"^{rules[0].Replace("8", $"({Memory[42]})+").Replace("11", $"(?<G>{Memory[42]})+(?<-G> {Memory[31]})+").Replace(" ", "")}$";
            var regex = new Regex(pattern, RegexOptions.Compiled);

            return $"{input[1].Count(x => regex.IsMatch(x))}";
        }

        private static string GetRule(Dictionary<int, string> rules, int ruleNumber)
        {
            if (!Memory.ContainsKey(ruleNumber))
            {
                if (rules[ruleNumber][0] == '"')
                {
                    Memory[ruleNumber] = rules[ruleNumber][1..^1];
                }
                else if (rules[ruleNumber].Contains('|'))
                {
                    (string left, string right) = rules[ruleNumber].Split('|');
                    string leftOptions = string.Concat(left.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => GetRule(rules, int.Parse(x))));
                    string rightOptions = string.Concat(right.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => GetRule(rules, int.Parse(x))));
                    Memory[ruleNumber] = $"({leftOptions}|{rightOptions})";
                }
                else
                {
                    Memory[ruleNumber] = string.Concat(rules[ruleNumber].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => GetRule(rules, int.Parse(x))));
                }
            }

            return Memory[ruleNumber];
        }
    }
}
