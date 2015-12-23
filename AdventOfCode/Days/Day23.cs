// stylecop.header
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day23 : Day
    {
        public override object ProcessFirst()
        {
            var registers = new Dictionary<string, ulong>
                {
                    { "a", 0 },
                    { "b", 0 }
                };

            RunBinary(registers, this.Lines.ToList());

            return registers["b"];
        }

        public override object ProcessSecond()
        {
            var registers = new Dictionary<string, ulong>
                {
                    { "a", 1 },
                    { "b", 0 }
                };

            RunBinary(registers, this.Lines.ToList());

            return registers["b"];
        }

        private static void RunBinary(Dictionary<string, ulong> registers, IList<string> lines)
        {
            var curPos = 0;
            while (curPos < lines.Count)
            {
                var tokens = lines[curPos].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                switch (tokens[0])
                {
                    case "hlf":
                        registers[tokens[1]] = registers[tokens[1]] / 2;
                        curPos++;
                        break;

                    case "tpl":
                        registers[tokens[1]] = registers[tokens[1]] * 3;
                        curPos++;
                        break;

                    case "inc":
                        registers[tokens[1]] = registers[tokens[1]] + 1;
                        curPos++;
                        break;

                    case "jmp":
                        curPos += int.Parse(tokens[1]);
                        break;

                    case "jie":
                        curPos += (registers[tokens[1]] & 1) == 0 ? int.Parse(tokens[2]) : 1;
                        break;

                    case "jio":
                        curPos += registers[tokens[1]] == 1 ? int.Parse(tokens[2]) : 1;
                        break;
                }
            }
        }
    }
}