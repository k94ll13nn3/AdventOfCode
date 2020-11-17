using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day17 : Day
    {
        public override string Title => "Set and Forget";

        public override string ProcessFirst()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            interpreter.Run(null);

            int size = interpreter.Outputs.IndexOf(10) + 1;
            char[,] map = new char[interpreter.Outputs.Count / size, size];
            int cursor = 0;
            for (int i = 0; i < interpreter.Outputs.Count / size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j] = (char)interpreter.Outputs[cursor];
                    cursor++;
                }
            }

            int count = 0;
            for (int i = 1; i < (interpreter.Outputs.Count / size) - 1; i++)
            {
                for (int j = 1; j < size - 1; j++)
                {
                    if (map[i, j] == '#' && map[i + 1, j] == '#' && map[i - 1, j] == '#' && map[i, j + 1] == '#' && map[i, j - 1] == '#')
                    {
                        count += i * j;
                    }
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            interpreter.Run(null);

            int size = interpreter.Outputs.IndexOf(10) + 1;
            char[,] map = new char[interpreter.Outputs.Count / size, size];
            int cursor = 0;
            for (int i = 0; i < interpreter.Outputs.Count / size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j] = (char)interpreter.Outputs[cursor];
                    cursor++;
                }
            }

            int indexOfVacuum = interpreter.Outputs.FindIndex(x => x != 46 && x != 10 && x != 35);
            (int x, int y) position = (indexOfVacuum / size, indexOfVacuum % size);
            char k = map[indexOfVacuum / size, indexOfVacuum % size];
            var directions = new List<char> { '^', '>', 'v', '<' };
            int directionIndex = directions.IndexOf(k);

            var commands = new List<string>();
            int currentCount = 0;
            bool continueLoop = true;
            while (continueLoop)
            {
                continueLoop = false;
                (int x, int y) nextPosition = GetNextPosition(directionIndex, position);
                if (ValidatePoint(nextPosition))
                {
                    currentCount++;
                    position = nextPosition;
                    continueLoop = true;
                }
                else
                {
                    if (currentCount != 0)
                    {
                        commands.Add(currentCount.ToString());
                    }

                    currentCount = 0;

                    if (ValidatePoint(GetNextPosition((directionIndex + 1) % 4, position)))
                    {
                        continueLoop = true;
                        directionIndex = (directionIndex + 1) % 4;
                        commands.Add("R");
                    }

                    if (ValidatePoint(GetNextPosition((directionIndex + 3) % 4, position)))
                    {
                        continueLoop = true;
                        directionIndex = (directionIndex + 3) % 4;
                        commands.Add("L");
                    }
                }
            }

            // commands gives us the following string, that can be easily split into 3 patterns:
            // L,4,L,6,L,8,L,12,L,8,R,12,L,12,L,8,R,12,L,12,L,4,L,6,L,8,L,12,L,8,R,12,L,12,R,12,L,6,L,6,L,8,L,4,L,6,L,8,L,12,R,12,L,6,L,6,L,8,L,8,R,12,L,12,R,12,L,6,L,6,L,8
            //
            // A,B,B,A,B,C,A,C,B,C
            //
            // A = L,4,L,6,L,8,L,12
            // B = L,8,R,12,L,12
            // C = R,12,L,6,L,6,L,8

            long[] program = GetContentAsLongArray(',');
            program[0] = 2;
            interpreter = new IntcodeInterpreter(program);
            interpreter.Run(null);
            int ind = 0;
            var commandsSplitted = "A,B,B,A,B,C,A,C,B,C\nL,4,L,6,L,8,L,12\nL,8,R,12,L,12\nR,12,L,6,L,6,L,8\nn\n".Select(x => (long)x).ToList();
            while (interpreter.State == IntcodeInterpreter.InputNeeded)
            {
                interpreter.Run(commandsSplitted[ind]);
                ind++;
            }
            return interpreter.Outputs[^1].ToString();

            bool ValidatePoint((int x, int y) point)
            {
                return point.x >= 0
                        && point.x < interpreter.Outputs.Count / size
                        && point.y >= 0
                        && point.y < size
                        && map[point.x, point.y] == '#';
            }

            (int x, int y) GetNextPosition(int nextIndex, (int x, int y) point)
            {
                return directions[nextIndex] switch
                {
                    '^' => (point.x - 1, point.y),
                    '>' => (point.x, point.y + 1),
                    'v' => (point.x + 1, point.y),
                    '<' => (point.x, point.y - 1),
                    _ => throw new InvalidOperationException(),
                };
            }
        }
    }
}
