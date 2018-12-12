using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day12 : Day
    {
        public override string Title => "Subterranean Sustainability";

        public override string ProcessFirst()
        {
            var lines = GetLinesAsStrings();
            var state = new string('.', 30) + lines.First().Replace("initial state: ", string.Empty) + new string('.', 30);

            var patterns = new Dictionary<string, string>();
            foreach (var line in lines.Skip(2))
            {
                var parts = line.Split(" =>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                patterns[parts[0]] = parts[1];
            }

            for (int i = 0; i < 20; i++)
            {
                state = Compute(state);
            }

            return state.Select((c, i) => c == '#' ? i - 30 : 0).Sum().ToString();

            string Compute(ReadOnlySpan<char> input)
            {
                var newState = new StringBuilder(".."); ;
                for (int j = 2; j < input.Length - 2; j++)
                {
                    var key = input.Slice(j - 2, 5).ToString();
                    if (patterns.ContainsKey(key))
                    {
                        newState.Append(patterns[key]);
                    }
                    else
                    {
                        newState.Append(".");

                    }
                }

                newState.Append("..");

                return newState.ToString();
            }
        }

        public override string ProcessSecond()
        {
            var lines = GetLinesAsStrings();
            var state = new string('.', 30) + lines.First().Replace("initial state: ", string.Empty) + new string('.', 200);

            var patterns = new Dictionary<string, string>();
            foreach (var line in lines.Skip(2))
            {
                var parts = line.Split(" =>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                patterns[parts[0]] = parts[1];
            }

            var counts = new Stack<int>();
            for (ulong i = 0; i < 50000000000; i++)
            {
                counts.Push(state.Count(c => c == '#'));
                if (counts.Count > 10 && counts.Take(10).All(x => x == counts.First()))
                {
                    ulong currentCount = (ulong)state.Select((c, z) => c == '#' ? z - 30 : 0).Sum();
                    return (currentCount + (ulong)counts.First() * (50000000000 - i)).ToString();
                }
                state = Compute(state);
            }

            return 0.ToString();

            string Compute(ReadOnlySpan<char> input)
            {
                var newState = new StringBuilder(".."); ;
                for (int j = 2; j < input.Length - 2; j++)
                {
                    var key = input.Slice(j - 2, 5).ToString();
                    if (patterns.ContainsKey(key))
                    {
                        newState.Append(patterns[key]);
                    }
                    else
                    {
                        newState.Append(".");

                    }
                }

                newState.Append("..");

                return newState.ToString();
            }
        }
    }
}