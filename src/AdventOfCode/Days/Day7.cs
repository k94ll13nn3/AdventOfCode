using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    [SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "FP, see https://github.com/SonarSource/sonar-dotnet/issues/3126")]
    public class Day7 : Day
    {
        public override string Title => "Handy Haversacks";

        public override string ProcessFirst()
        {
            Dictionary<string, Dictionary<string, int>> bags = GetBags();
            var bagsCount = new Dictionary<string, bool>();

            return $"{bags.Count(x => ContainsShinyBag(x.Key))}";

            bool ContainsShinyBag(string bag)
            {
                if (!bagsCount.ContainsKey(bag))
                {
                    Dictionary<string, int> bagContent = bags[bag];
                    bool contains = false;
                    foreach (KeyValuePair<string, int> item in bagContent)
                    {
                        if (item.Key == "shiny gold")
                        {
                            contains = true;
                            break;
                        }
                        else
                        {
                            contains = contains || ContainsShinyBag(item.Key);
                        }
                    }

                    bagsCount[bag] = contains;
                }

                return bagsCount[bag];
            }
        }

        public override string ProcessSecond()
        {
            Dictionary<string, Dictionary<string, int>> bags = GetBags();
            var bagsCount = new Dictionary<string, int>();

            return $"{CountBags("shiny gold") - 1}"; // -1 because the shiny bag is counted in the method but does not need to be counted in the result.

            int CountBags(string bag)
            {
                if (!bagsCount.ContainsKey(bag))
                {
                    Dictionary<string, int> bagContent = bags[bag];
                    int count = 1;
                    foreach (KeyValuePair<string, int> item in bagContent)
                    {
                        count += item.Value * CountBags(item.Key);
                    }

                    bagsCount[bag] = count;
                }

                return bagsCount[bag];
            }
        }

        private Dictionary<string, Dictionary<string, int>> GetBags()
        {
            var bags = new Dictionary<string, Dictionary<string, int>>();
            foreach (string line in GetLines())
            {
                (string color, string content) = line.Split("contain");
                var bagContent = new Dictionary<string, int>();
                if (content.Trim() != "no other bags.")
                {
                    bagContent = content.Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToDictionary(x => x[1..^4].Trim(), x => int.Parse(x[0..1]));
                }

                bags[color[0..^5].Trim()] = bagContent;
            }

            return bags;
        }
    }
}
