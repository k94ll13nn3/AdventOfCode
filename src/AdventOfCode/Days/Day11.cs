using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day11 : Day
    {
        public override string Title => "Space Police";

        public override string ProcessFirst()
        {
            (_, int paintedPanelsCount) = Compute(0);

            return paintedPanelsCount.ToString();
        }

        [SuppressMessage("Major Code Smell", "S125", Justification = "Comments for the result.")]
        public override string ProcessSecond()
        {
            (long[,] map, int paintedPanelsCount) = Compute(1);

            // Uncomment for result.
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    //Console.Write(map[i, j] == 1 ? '\u2588' : ' ');
                }

                //Console.WriteLine();
            }

            return $"SEE DRAWING ({paintedPanelsCount})";
        }

        private (long[,] map, int paintedPanelsCount) Compute(long startingPanel)
        {
            const int center = 100;
            long[,] map = new long[center * 2, center * 2];

            var interpreter = new IntcodeInterpreter(GetContentAsLongArray(','));
            bool robotStopped = false;
            int i = center;
            int j = center;
            char[] directions = new[] { 'U', 'R', 'D', 'L' };
            int directionIndex = 0;
            var paintedPanels = new List<(int x, int y)>();
            map[i, j] = startingPanel;
            while (!robotStopped)
            {
                interpreter.Run(map[i, j]);
                map[i, j] = interpreter.Outputs[0];
                paintedPanels.Add((i, j));
                if (interpreter.Outputs[1] == 0)
                {
                    directionIndex--;
                }
                else
                {
                    directionIndex++;
                }

                directionIndex = (directionIndex + 4) % 4;

                switch (directions[directionIndex])
                {
                    case 'U':
                        i--;
                        break;

                    case 'D':
                        i++;
                        break;

                    case 'L':
                        j--;
                        break;

                    case 'R':
                        j++;
                        break;
                }

                robotStopped = interpreter.State == IntcodeInterpreter.Stopped;
            }

            return (map, paintedPanels.Distinct().Count());
        }
    }
}
