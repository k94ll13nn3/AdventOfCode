using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day16 : Day
    {
        public override string Title => "Chronal Classification";

        public override string ProcessFirst()
        {
            var lines = GetLinesAsStrings().ToList();
            var input = lines.TakeWhile((e, i) => e != lines[i + 1]).Where(x => x.Length > 0).ToList();//

            var count = 0;
            for (int i = 0; i < input.Count; i += 3)
            {
                var before = input[i].Substring(8).Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var values = input[i + 1].Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Skip(1).ToList();
                var after = input[i + 2].Substring(7).Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                var equalResultCount = 0;
                var ops = new[] { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtrt", "gtrr", "eqir", "eqrt", "eqrr" };
                foreach (var op in ops)
                {
                    if (Compute(op, values, before).SequenceEqual(after))
                    {
                        equalResultCount++;
                    }
                }

                if (equalResultCount >= 3)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        private IList<int> Compute(string op, IList<int> values, IList<int> startState)
        {
            var result = startState.ToList();
            switch (op)
            {
                case "addr":
                    result[values[2]] = startState[values[0]] + startState[values[1]];
                    break;
                case "addi":
                    result[values[2]] = startState[values[0]] + values[1];
                    break;
                case "mulr":
                    result[values[2]] = startState[values[0]] * startState[values[1]];
                    break;
                case "muli":
                    result[values[2]] = startState[values[0]] * values[1];
                    break;
                case "banr":
                    result[values[2]] = startState[values[0]] & startState[values[1]];
                    break;
                case "bani":
                    result[values[2]] = startState[values[0]] & values[1];
                    break;
                case "borr":
                    result[values[2]] = startState[values[0]] | startState[values[1]];
                    break;
                case "bori":
                    result[values[2]] = startState[values[0]] | values[1];
                    break;
                case "setr":
                    result[values[2]] = startState[values[0]];
                    break;
                case "seti":
                    result[values[2]] = values[0];
                    break;
                case "gtir":
                    result[values[2]] = values[0] > startState[values[1]] ? 1 : 0;
                    break;
                case "gtrt":
                    result[values[2]] = startState[values[0]] > values[1] ? 1 : 0;
                    break;
                case "gtrr":
                    result[values[2]] = startState[values[0]] > startState[values[1]] ? 1 : 0;
                    break;
                case "eqir":
                    result[values[2]] = values[0] == startState[values[1]] ? 1 : 0;
                    break;
                case "eqrt":
                    result[values[2]] = startState[values[0]] == values[1] ? 1 : 0;
                    break;
                case "eqrr":
                    result[values[2]] = startState[values[0]] == startState[values[1]] ? 1 : 0;
                    break;
            }

            return result;
        }

        public override string ProcessSecond()
        {
            var lines = GetLinesAsStrings().ToList();
            var input = lines.TakeWhile((e, i) => e != lines[i + 1]).Where(x => x.Length > 0).ToList();//
            var program = lines.SkipWhile((e, i) => e != lines[i + 2]).Where(x => x.Length > 0);

            var ops = new[] { "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtrt", "gtrr", "eqir", "eqrt", "eqrr" };
            var table = ops.ToDictionary(x => x, x => new List<int>());

            for (int i = 0; i < input.Count; i += 3)
            {
                var before = input[i].Substring(8).Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var values = input[i + 1].Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                var after = input[i + 2].Substring(7).Split(new[] { ',', '[', ']', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                foreach (var op in ops)
                {
                    if (Compute(op, values.Skip(1).ToList(), before).SequenceEqual(after) && !table[op].Contains(values[0]))
                    {
                        table[op].Add(values[0]);
                    }
                }
            }

            var valuesToRemove = new List<int>();
            while (table.Any(x => x.Value.Count > 1))
            {
                valuesToRemove.AddRange(table.Where(x => x.Value.Count == 1).Select(x => x.Value[0]));
                foreach (var t in table.Where(x => x.Value.Count > 1))
                {
                    t.Value.RemoveAll(valuesToRemove.Contains);
                }
            }

            var numberToOp = table.ToDictionary(x => x.Value[0], x => x.Key);

            IList<int> startState = new List<int> { 0, 0, 0, 0 };
            foreach (var line in program)
            {
                var parts = line.Split(' ').Select(int.Parse);
                startState = Compute(numberToOp[parts.First()], parts.Skip(1).ToList(), startState);
            }

            return startState[0].ToString();
        }
    }
}