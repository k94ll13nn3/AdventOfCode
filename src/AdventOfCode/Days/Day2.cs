using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day2 : Day
    {
        public override string Title => "1202 Program Alarm";

        public override string ProcessFirst()
        {
            int[] program = GetContentAsIntArray(',');
            program[1] = 12;
            program[2] = 2;

            return Compute(program).ToString();
        }

        public override string ProcessSecond()
        {
            int[] program = GetContentAsIntArray(',');
            (int noun, int verb, int _) = GetSequence().First(x => x.result == 19690720);
            return ((noun * 100) + verb).ToString();

            IEnumerable<(int noun, int verb, int result)> GetSequence()
            {
                for (int i = 0; i < 99; i++)
                {
                    for (int j = 0; j < 99; j++)
                    {
                        int[] array = new int[program.Length];
                        program.CopyTo(array, 0);
                        array[1] = i;
                        array[2] = j;
                        yield return (i, j, Compute(array));
                    }
                }
            }
        }

        private static int Compute(int[] program)
        {
            int pointer = 0;
            while (program[pointer] != 99)
            {
                program[program[pointer + 3]] = program[pointer] == 1
                    ? program[program[pointer + 1]] + program[program[pointer + 2]]
                    : program[program[pointer + 1]] * program[program[pointer + 2]];

                pointer += 4;
            }

            return program[0];
        }
    }
}
