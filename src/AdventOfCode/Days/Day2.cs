using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day2 : Day
    {
        public override string Title => "1202 Program Alarm";

        public override string ProcessFirst()
        {
            long[] program = GetContentAsLongArray(',');
            program[1] = 12;
            program[2] = 2;

            var interpreter = new IntcodeInterpreter(program);
            interpreter.Run(null);

            return interpreter[0].ToString();
        }

        public override string ProcessSecond()
        {
            long[] program = GetContentAsLongArray(',');
            (long noun, long verb, _) = GetSequence().First(x => x.result == 19690720);
            return ((noun * 100) + verb).ToString();

            IEnumerable<(long noun, long verb, long result)> GetSequence()
            {
                for (int i = 0; i < 99; i++)
                {
                    for (int j = 0; j < 99; j++)
                    {
                        long[] array = new long[program.Length];
                        program.CopyTo(array, 0);
                        array[1] = i;
                        array[2] = j;
                        var interpreter = new IntcodeInterpreter(array);
                        interpreter.Run(null);
                        yield return (i, j, interpreter[0]);
                    }
                }
            }
        }
    }
}
