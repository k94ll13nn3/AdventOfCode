using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day19 : Day
    {
        private static readonly Dictionary<int, List<string>> Memory = new();
        public override string Title => "Monster Messages";

        public override string ProcessFirst()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var rules = input[0].Select(x => x.Split(':')).ToDictionary(x => int.Parse(x[0].Trim()), x => x[1].Trim());
            HashSet<string> allPossibleRules = GetRule(rules, 0).ToHashSet();

            HashSet<string> inputHashSet = input[1].ToHashSet();
            inputHashSet.IntersectWith(allPossibleRules);

            return $"{inputHashSet.Count}";
        }

        public override string ProcessSecond()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            var rules = input[0].Select(x => x.Split(':')).ToDictionary(x => int.Parse(x[0].Trim()), x => x[1].Trim());

            if (rules[0] != "8 11")
            {
                throw new InvalidOperationException("cannot handle this input");
            }

            rules[8] = "42 | 42 8";
            rules[11] = "42 31 | 42 11 31";

            List<string> rule42 = GetRule(rules, 42);
            List<string> rule31 = GetRule(rules, 31);
            int count = 0;
            foreach (string word in input[1])
            {
                int length = rule42[0].Length;
                int i = 0;
                int count42 = 0;
                while (i + length <= word.Length && rule42.Contains(word[i..(i + length)]))
                {
                    i += length;
                    count42++;
                }

                if (i < 2 * rule42[0].Length || i >= word.Length)
                {
                    // At least two 42 (one for 8 and one for 11) is needed and one 31 (for the end of 11).
                    continue;
                }

                int count31 = 0;
                length = rule31[0].Length;
                while (i + length <= word.Length && rule31.Contains(word[i..(i + length)]))
                {
                    i += length;
                    count31++;
                }

                if (i == word.Length && count31 < count42)
                {
                    count++;
                }
            }

            return $"{count}";
        }

        private static List<string> GetRule(Dictionary<int, string> rules, int ruleNumber)
        {
            if (!Memory.ContainsKey(ruleNumber))
            {
                if (rules[ruleNumber][0] == '"')
                {
                    Memory[ruleNumber] = new List<string> { rules[ruleNumber][1..^1] };
                }
                else if (rules[ruleNumber].Contains('|'))
                {
                    (string left, string right) = rules[ruleNumber].Split('|');
                    var res = new List<string>();
                    res.AddRange(GetAllPossiblities(left.Trim().Split(' ').Select(x => GetRule(rules, int.Parse(x))).ToArray()));
                    res.AddRange(GetAllPossiblities(right.Trim().Split(' ').Select(x => GetRule(rules, int.Parse(x))).ToArray()));
                    Memory[ruleNumber] = res;
                }
                else
                {
                    List<string>[] parts = rules[ruleNumber].Split(' ').Select(x => GetRule(rules, int.Parse(x))).ToArray();

                    Memory[ruleNumber] = GetAllPossiblities(parts);
                }
            }

            return Memory[ruleNumber];
        }

        private static List<string> GetAllPossiblities(List<string>[] input)
        {
            if (input.Length == 1)
            {
                return input[0];
            }

            var res = new List<string>();
            foreach (string firstInput in input[0])
            {
                foreach (string possibility in GetAllPossiblities(input[1..]))
                {
                    res.Add(firstInput + possibility);
                }
            }

            return res;
        }
    }
}
