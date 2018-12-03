using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day3 : Day
    {
        private readonly Regex claimPattern = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)", RegexOptions.Compiled);

        public override string Title => "No Matter How You Slice It";

        public override string ProcessFirst()
        {
            var fabric = new int[1000, 1000];
            var lines = GetLinesAsStrings();
            var count = 0;
            foreach (var line in lines)
            {
                var claim = ParseClaim(line);
                for (int i = claim.x; i < claim.x + claim.height; i++)
                {
                    for (int j = claim.y; j < claim.y + claim.width; j++)
                    {
                        fabric[i, j] = fabric[i, j] + 1;
                        if (fabric[i, j] == 2)
                        {
                            count++;
                        }
                    }
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            var fabric = new (int value, int claim)[1000, 1000];
            var lines = GetLinesAsStrings();
            var nonOverlapingClaims = new HashSet<int>();
            foreach (var line in lines)
            {
                var overlap = false;
                var claim = ParseClaim(line);
                for (int i = claim.x; i < claim.x + claim.height; i++)
                {
                    for (int j = claim.y; j < claim.y + claim.width; j++)
                    {
                        fabric[i, j].value = fabric[i, j].value + 1;
                        if (fabric[i, j].value >= 2)
                        {
                            overlap = true;
                            nonOverlapingClaims.Remove(fabric[i, j].claim);
                        }
                        fabric[i, j].claim = claim.claimNumber;
                    }
                }

                if (!overlap)
                {
                    nonOverlapingClaims.Add(claim.claimNumber);
                }
            }

            return nonOverlapingClaims.First().ToString();
        }

        private (int claimNumber, int x, int y, int height, int width) ParseClaim(string claim)
        {
            Match match = claimPattern.Match(claim);
            if (match.Success)
            {
                return (
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value),
                    int.Parse(match.Groups[5].Value));
            }

            return (0, 0, 0, 0, 0);
        }
    }
}