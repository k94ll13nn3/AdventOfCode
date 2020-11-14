using System;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
    internal static class IntcodeInterpreter
    {
        public static (string state, List<int> outputs, int pointer) Run(int[] program, int input)
        {
            return Run(program, new Queue<int>(new[] { input }));
        }

        public static (string state, List<int> outputs, int pointer) Run(int[] program, Queue<int>? inputs, int pointer = 0)
        {
            List<int> outputs = new();
            int par1 = 0;
            int par2 = 0;
            while (true)
            {
                string instruction = program[pointer].ToString().PadLeft(5, '0');
                switch (instruction[^2..])
                {
                    case "99":
                        return ("ENDED", outputs, pointer);

                    case "01":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        program[program[pointer + 3]] = par1 + par2;
                        pointer += 4;
                        break;

                    case "02":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        program[program[pointer + 3]] = par1 * par2;
                        pointer += 4;
                        break;

                    case "03":
                        if (inputs?.Count > 0)
                        {
                            program[program[pointer + 1]] = inputs.Dequeue();
                            pointer += 2;
                        }
                        else
                        {
                            return ("INPUT_NEEDED", outputs, pointer);
                        }

                        break;

                    case "04":
                        outputs.Add(instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1]);
                        pointer += 2;
                        break;

                    case "05":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        if (par1 != 0)
                        {
                            pointer = par2;
                        }
                        else
                        {
                            pointer += 3;
                        }

                        break;

                    case "06":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        if (par1 == 0)
                        {
                            pointer = par2;
                        }
                        else
                        {
                            pointer += 3;
                        }

                        break;

                    case "07":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        program[program[pointer + 3]] = par1 < par2 ? 1 : 0;
                        pointer += 4;
                        break;

                    case "08":
                        par1 = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
                        par2 = instruction[^4] == '0' ? program[program[pointer + 2]] : program[pointer + 2];
                        program[program[pointer + 3]] = par1 == par2 ? 1 : 0;
                        pointer += 4;
                        break;
                }
            }

            throw new InvalidOperationException("Program incomplete");
        }
    }
}
