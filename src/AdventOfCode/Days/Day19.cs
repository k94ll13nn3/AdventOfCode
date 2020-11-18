using System.Collections.Generic;

namespace AdventOfCode.Days
{
    public class Day19 : Day
    {
        public override string Title => "Tractor Beam";

        public override string ProcessFirst()
        {
            int count = 0;
            List<int> firstJ = new();
            for (int i = 0; i < 50; i++)
            {
                bool seen = false;
                for (int j = i - 1 < 0 ? 0 : firstJ[i - 1]; j < 50; j++)
                {
                    var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
                    interpreter.Run(i);
                    interpreter.Run(j);
                    if (interpreter.Outputs[0] == 1)
                    {
                        count++;
                        seen = true;
                        if (firstJ.Count == i)
                        {
                            firstJ.Add(j);
                        }
                    }

                    if (interpreter.Outputs[0] == 0 && seen)
                    {
                        break;
                    }
                }
                if (firstJ.Count == i)
                {
                    firstJ.Add(0);
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            long[] program = GetContentAsLongArray(',');
            bool shipFound = false;
            const int offset = 100; // i & j must be positives for the intcode program, so more than the shipsize
            int i = offset;
            int foundJ = offset;
            const int shipSize = 99;
            (int x, int y) position = (-1, -1);

            while (!shipFound)
            {
                for (int j = foundJ; !shipFound; j++)
                {
                    var interpreter = new IntcodeInterpreter(program);
                    interpreter.Run(i);
                    interpreter.Run(j);
                    if (interpreter.Outputs[0] == 1)
                    {
                        foundJ = j;
                        position = (i, j);
                        break;
                    }
                }
                if (CanShipFit(position))
                {
                    shipFound = true;
                }

                i++;
            }

            return (((position.x - shipSize) * 10000) + position.y).ToString();

            bool CanShipFit((int x, int y) position)
            {
                var interpreter = new IntcodeInterpreter(program);
                interpreter.Run(position.x - shipSize);
                interpreter.Run(position.y + shipSize);

                if (interpreter.Outputs.Count == 1 && interpreter.Outputs[0] == 1)
                {
                    interpreter = new IntcodeInterpreter(program);
                    interpreter.Run(position.x - shipSize);
                    interpreter.Run(position.y);

                    if (interpreter.Outputs.Count == 1 && interpreter.Outputs[0] == 1)
                    {
                        interpreter = new IntcodeInterpreter(program);
                        interpreter.Run(position.x);
                        interpreter.Run(position.y + shipSize);

                        if (interpreter.Outputs.Count == 1 && interpreter.Outputs[0] == 1)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
