// stylecop.header

using System;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day18 : Day
    {
        private const int GridSize = 100;

        public override object ProcessFirst()
        {
            var grid = ProcessInput(this.Lines.ToArray());

            for (int i = 0; i < GridSize; i++)
            {
                grid = ComputeNextGrid(grid);
            }

            return grid.Select(l => l.Count(c => c)).Sum();
        }

        public override object ProcessSecond()
        {
            var grid = ProcessInput(this.Lines.ToArray());
            ResetCorners(grid);

            for (int i = 0; i < GridSize; i++)
            {
                grid = ComputeNextGrid(grid);
                ResetCorners(grid);
            }

            return grid.Select(l => l.Count(c => c)).Sum();
        }

        private static bool ComputeNewState(int i, int j, bool[][] grid)
        {
            var countOnLights = 0;

            for (int k = Math.Max(0, i - 1); k <= Math.Min(GridSize - 1, i + 1); k++)
            {
                for (int l = Math.Max(0, j - 1); l <= Math.Min(GridSize - 1, j + 1); l++)
                {
                    if (grid[k][l])
                    {
                        countOnLights++;
                    }
                }
            }

            return countOnLights == 3 || (grid[i][j] && countOnLights == 4);
        }

        private static bool[][] ComputeNextGrid(bool[][] grid)
        {
            var nextGrid = new bool[GridSize][];
            for (int i = 0; i < GridSize; i++)
            {
                nextGrid[i] = new bool[GridSize];
                for (int j = 0; j < GridSize; j++)
                {
                    nextGrid[i][j] = ComputeNewState(i, j, grid);
                }
            }

            return nextGrid;
        }

        private static bool[][] ProcessInput(string[] lines)
        {
            var grid = new bool[GridSize][];
            for (int i = 0; i < lines.Length; i++)
            {
                grid[i] = lines[i].Select(c => c == '#').ToArray();
            }

            return grid;
        }

        private static void ResetCorners(bool[][] grid)
        {
            grid[0][0] = true;
            grid[0][GridSize - 1] = true;
            grid[GridSize - 1][0] = true;
            grid[GridSize - 1][GridSize - 1] = true;
        }
    }
}