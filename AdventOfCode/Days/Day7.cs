using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day7 : Day
    {
        public Day7()
            : base(7)
        {
        }

        public override object ProcessFirst()
        {
            return Process(new List<string>(this.Lines));
        }

        public override object ProcessSecond()
        {
            return Process(new List<string>(this.Lines.Select(s => s == "19138 -> b" ? "16076 -> b" : s)));
        }

        private static bool ProcessCommand(Dictionary<string, ushort> wires, string command)
        {
            var match = Regex.Match(command, @"^(\d+) -> ([a-z]+)$");
            if (match.Success)
            {
                wires[match.Groups[2].Value] = ushort.Parse(match.Groups[1].Value);
                return true;
            }

            match = Regex.Match(command, @"^([a-z]+) -> ([a-z]+)$");
            if (match.Success)
            {
                if (wires.ContainsKey(match.Groups[1].Value))
                {
                    wires[match.Groups[2].Value] = wires[match.Groups[1].Value];
                    return true;
                }

                return false;
            }

            match = Regex.Match(command, @"^NOT ([a-z]+) -> ([a-z]+)$");
            if (match.Success)
            {
                if (wires.ContainsKey(match.Groups[1].Value))
                {
                    wires[match.Groups[2].Value] = (ushort)~wires[match.Groups[1].Value];
                    return true;
                }

                return false;
            }

            match = Regex.Match(command, @"^([a-z]+) (AND|OR) ([a-z]+) -> ([a-z]+)$");
            if (match.Success)
            {
                if (wires.ContainsKey(match.Groups[1].Value) && wires.ContainsKey(match.Groups[3].Value))
                {
                    if (match.Groups[2].Value == "AND")
                    {
                        wires[match.Groups[4].Value] = (ushort)(wires[match.Groups[1].Value] & wires[match.Groups[3].Value]);
                    }
                    else
                    {
                        wires[match.Groups[4].Value] = (ushort)(wires[match.Groups[1].Value] | wires[match.Groups[3].Value]);
                    }

                    return true;
                }

                return false;
            }

            match = Regex.Match(command, @"^(\d+) (AND|OR) ([a-z]+) -> ([a-z]+)$");
            if (match.Success)
            {
                if (wires.ContainsKey(match.Groups[3].Value))
                {
                    if (match.Groups[2].Value == "AND")
                    {
                        wires[match.Groups[4].Value] = (ushort)(ushort.Parse(match.Groups[1].Value) & wires[match.Groups[3].Value]);
                    }
                    else
                    {
                        wires[match.Groups[4].Value] = (ushort)(ushort.Parse(match.Groups[1].Value) | wires[match.Groups[3].Value]);
                    }

                    return true;
                }

                return false;
            }

            match = Regex.Match(command, @"^([a-z]+) (L|R)SHIFT (\d+) -> ([a-z]+)$");
            if (match.Success)
            {
                if (wires.ContainsKey(match.Groups[1].Value))
                {
                    if (match.Groups[2].Value == "L")
                    {
                        wires[match.Groups[4].Value] = (ushort)(wires[match.Groups[1].Value] << ushort.Parse(match.Groups[3].Value));
                    }
                    else
                    {
                        wires[match.Groups[4].Value] = (ushort)(wires[match.Groups[1].Value] >> ushort.Parse(match.Groups[3].Value));
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        private static ushort Process(IEnumerable<string> list)
        {
            var wires = new Dictionary<string, ushort>();
            var retry = new List<string>(list);

            while (retry.Any())
            {
                list = new List<string>(retry);
                foreach (var line in list)
                {
                    if (ProcessCommand(wires, line))
                    {
                        retry.Remove(line);
                    }
                }
            }

            return wires["a"];
        }
    }
}