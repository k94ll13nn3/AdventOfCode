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

            new IntcodeInterpreter(program).Run(null);

            return program[0].ToString();
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
                        new IntcodeInterpreter(array).Run(null);
                        yield return (i, j, array[0]);
                    }
                }
            }
        }
    }
}
