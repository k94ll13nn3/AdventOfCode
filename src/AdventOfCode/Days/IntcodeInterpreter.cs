namespace AdventOfCode.Days
{
    internal static class IntcodeInterpreter
    {
        public static int Run(int[] program, int input)
        {
            int pointer = 0;
            int output = 0;
            int par1 = 0;
            int par2 = 0;
            bool stop = false;
            while (!stop)
            {
                string instruction = $"00000{program[pointer]}";
                switch (instruction[^2..])
                {
                    case "99":
                        stop = true;
                        break;

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
                        program[program[pointer + 1]] = input;
                        pointer += 2;
                        break;

                    case "04":
                        output = instruction[^3] == '0' ? program[program[pointer + 1]] : program[pointer + 1];
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

            return output;
        }
    }
}
