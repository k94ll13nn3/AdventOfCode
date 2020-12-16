using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day16 : Day
    {
        public override string Title => "Ticket Translation";

        public override string ProcessFirst()
        {
            (IEnumerable<(string name, int r1a, int r1b, int r2a, int r2b)> rules, IEnumerable<long[]> tickets, _) = ParseInput();

            long sum = 0;
            foreach (long[] ticket in tickets)
            {
                foreach (long ticketValue in ticket)
                {
                    bool found = false;
                    foreach ((string name, int r1a, int r1b, int r2a, int r2b) in rules)
                    {
                        if ((ticketValue >= r1a && ticketValue <= r1b) || (ticketValue >= r2a && ticketValue <= r2b))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        sum += ticketValue;
                    }
                }
            }

            return $"{sum}";
        }

        public override string ProcessSecond()
        {
            (IEnumerable<(string name, int r1a, int r1b, int r2a, int r2b)> rules, IEnumerable<long[]> tickets, long[] myTicket) = ParseInput();

            // For each seat in each ticket, list all possible rules.
            var goodTickets = new List<List<(long value, List<string> rules)>>();
            foreach (long[] ticket in tickets)
            {
                var ticketWithRules = new List<(long value, List<string> rules)>();
                foreach (long ticketValue in ticket)
                {
                    var possibleRule = new List<string>();
                    foreach ((string name, int r1a, int r1b, int r2a, int r2b) in rules)
                    {
                        if ((ticketValue >= r1a && ticketValue <= r1b) || (ticketValue >= r2a && ticketValue <= r2b))
                        {
                            possibleRule.Add(name);
                        }
                    }

                    if (possibleRule.Count > 0)
                    {
                        ticketWithRules.Add((ticketValue, possibleRule));
                    }
                }

                if (ticketWithRules.Count == ticket.Length)
                {
                    goodTickets.Add(ticketWithRules);
                }
            }

            // For each index, list all possible rules that match each ticket.
            var rulesForIndex = new List<(int ind, List<string> rules)>();
            for (int j = 0; j < goodTickets[0].Count; j++)
            {
                IEnumerable<string> k = new List<string>(goodTickets[0][j].rules);
                for (int i = 1; i < goodTickets.Count; i++)
                {
                    k = k.Intersect(goodTickets[i][j].rules);
                }
                rulesForIndex.Add((j, k.ToList()));
            }

            // Search for the index that has only one rule, save the association, and remove this rule/index from the list.
            // Do this until no more rule is present.
            var ruleByIndex = new Dictionary<string, int>();
            while (rulesForIndex.Count > 0)
            {
                (int ind, List<string> rules) l = rulesForIndex.First(x => x.rules.Count == 1);
                ruleByIndex[l.rules[0]] = l.ind;

                rulesForIndex.Remove(l);

                foreach ((int ind, List<string> rules) item in rulesForIndex)
                {
                    item.rules.Remove(l.rules[0]);
                }
            }

            long res = ruleByIndex.Where(x => x.Key.StartsWith("departure", StringComparison.OrdinalIgnoreCase)).Select(x => myTicket[x.Value]).Aggregate(1L, (a, b) => a * b);

            return $"{res}";
        }

        private (IEnumerable<(string name, int r1a, int r1b, int r2a, int r2b)> rules, IEnumerable<long[]> tickets, long[] myTicket) ParseInput()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .ToList();

            IEnumerable<(string name, int r1a, int r1b, int r2a, int r2b)> rules = input[0].Select(x =>
            {
                (string name, string rule1Min, string rule1Max, string rule2Min, string rule2Max) = x.Replace(" or ", "%").Split(new[] { ':', '%', '-' });
                return (name, int.Parse(rule1Min), int.Parse(rule1Max), int.Parse(rule2Min), int.Parse(rule2Max));
            });

            IEnumerable<long[]> tickets = input[2].Skip(1).Select(x => Array.ConvertAll(x.Split(','), long.Parse));

            long[] myTicket = Array.ConvertAll(input[1][1].Split(','), long.Parse);

            return (rules, tickets, myTicket);
        }
    }
}
