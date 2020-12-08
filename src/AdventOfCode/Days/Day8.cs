using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day8 : Day
    {
        public override string Title => "Handheld Halting";

        public override string ProcessFirst()
        {
            var instructions = GetLines()
                .Select(x => { (string operation, string argument) = x.Split(' '); return (operation, argument: int.Parse(argument)); })
                .ToList();

            return $"{ExecuteProgram(instructions).loopResult}";
        }

        public override string ProcessSecond()
        {
            var instructions = GetLines()
                .Select(x => { (string operation, string argument) = x.Split(' '); return (operation, argument: int.Parse(argument)); })
                .ToList();

            int result = 0;
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].operation == "jmp")
                {
                    instructions[i] = ("nop", instructions[i].argument);
                    result = Math.Max(ExecuteProgram(instructions).realResult, result);
                    instructions[i] = ("jmp", instructions[i].argument);
                }

                if (instructions[i].operation == "nop")
                {
                    instructions[i] = ("jmp", instructions[i].argument);
                    result = Math.Max(ExecuteProgram(instructions).realResult, result);
                    instructions[i] = ("nop", instructions[i].argument);
                }
            }

            return $"{result}";
        }

        private static (int realResult, int loopResult) ExecuteProgram(List<(string operation, int argument)> instructions)
        {
            var positions = new HashSet<int>();
            int acc = 0;
            int position = 0;
            while (position < instructions.Count)
            {
                positions.Add(position);
                switch (instructions[position].operation)
                {
                    case "acc":
                        acc += instructions[position].argument;
                        position++;
                        break;

                    case "nop":
                        position++;
                        break;

                    case "jmp":
                        position += instructions[position].argument;
                        break;
                }

                if (positions.Contains(position))
                {
                    return (0, acc);
                }
            }

            return (acc, 0);
        }
    }
}
