using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day17 : Day
    {
        public override string Title => "Conway Cubes";

        public override string ProcessFirst()
        {
            char[][] input = GetLines().Select(x => x.ToCharArray()).ToArray();
            char[][][] dimension = new char[10][][];
            const int padding = 20;
            const int numberOfCyles = 6;
            for (int i = 0; i < 10; i++)
            {
                dimension[i] = new char[input.Length + (padding * 2)][];
                for (int j = 0; j < input.Length + (padding * 2); j++)
                {
                    dimension[i][j] = new char[input[0].Length + (padding * 2)];
                }
            }

            for (int x = padding; x < input.Length + padding; x++)
            {
                for (int y = padding; y < input[0].Length + padding; y++)
                {
                    dimension[0][x][y] = input[x - padding][y - padding];
                }
            }

            for (int cycle = 0; cycle < numberOfCyles; cycle++)
            {
                char[][][] newDimension = dimension.Select(x => x.Select(y => y.ToArray()).ToArray()).ToArray();
                for (int z = 0; z < numberOfCyles + 1; z++)
                {
                    for (int x = 1; x < newDimension[0].Length - 1; x++)
                    {
                        for (int y = 1; y < newDimension[0][0].Length - 1; y++)
                        {
                            int neighbors = CountActiveNeighbors(z, x, y, dimension);
                            if (newDimension[z][x][y] == '\0')
                            {
                                newDimension[z][x][y] = '.';
                            }

                            if (newDimension[z][x][y] == '#' && neighbors != 2 && neighbors != 3)
                            {
                                newDimension[z][x][y] = '.';
                            }
                            else if (newDimension[z][x][y] == '.' && neighbors == 3)
                            {
                                newDimension[z][x][y] = '#';
                            }
                        }
                    }
                }

                dimension = newDimension.Select(x => x.Select(y => y.ToArray()).ToArray()).ToArray();
            }

            int count = CountActiveCells(dimension);

            return $"{count}";

            static int CountActiveNeighbors(int z, int x, int y, char[][][] dimension)
            {
                int count = 0;
                for (int zt = z - 1; zt <= z + 1; zt++)
                {
                    for (int xt = x - 1; xt <= x + 1; xt++)
                    {
                        for (int yt = y - 1; yt <= y + 1; yt++)
                        {
                            if (dimension[Math.Abs(zt)][xt][yt] == '#' && (zt, xt, yt) != (z, x, y))
                            {
                                count++;
                            }
                        }
                    }
                }

                return count;
            }

            static int CountActiveCells(char[][][] dimension)
            {
                int count = 0;
                for (int z = -6; z <= 6; z++)
                {
                    for (int i = 0; i < dimension[0].Length; i++)
                    {
                        for (int j = 0; j < dimension[0][0].Length; j++)
                        {
                            if (dimension[Math.Abs(z)][i][j] == '#')
                            {
                                count++;
                            }
                        }
                    }
                }

                return count;
            }
        }

        public override string ProcessSecond()
        {
            char[][] input = GetLines().Select(x => x.ToCharArray()).ToArray();
            char[][][][] dimension = new char[10][][][];
            const int padding = 20;
            for (int z = 0; z < 10; z++)
            {
                dimension[z] = new char[10][][];
                for (int w = 0; w < 10; w++)
                {
                    dimension[z][w] = new char[input.Length + (padding * 2)][];
                    for (int x = 0; x < input.Length + (padding * 2); x++)
                    {
                        dimension[z][w][x] = new char[input[0].Length + (padding * 2)];
                    }
                }
            }

            for (int x = padding; x < input.Length + padding; x++)
            {
                for (int y = padding; y < input[0].Length + padding; y++)
                {
                    dimension[0][0][x][y] = input[x - padding][y - padding];
                }
            }

            const int numberOfCyles = 6;
            for (int cycle = 0; cycle < numberOfCyles; cycle++)
            {
                char[][][][] newDimension = dimension.Select(x => x.Select(y => y.Select(t => t.ToArray()).ToArray()).ToArray()).ToArray();
                for (int z = 0; z < numberOfCyles + 1; z++)
                {
                    for (int w = 0; w < numberOfCyles + 1; w++)
                    {
                        for (int x = 1; x < newDimension[0][0].Length - 1; x++)
                        {
                            for (int y = 1; y < newDimension[0][0][0].Length - 1; y++)
                            {
                                int neighbors = CountActiveNeighbors(z, w, x, y, dimension);
                                if (newDimension[z][w][x][y] == '\0')
                                {
                                    newDimension[z][w][x][y] = '.';
                                }

                                if (newDimension[z][w][x][y] == '#' && neighbors != 2 && neighbors != 3)
                                {
                                    newDimension[z][w][x][y] = '.';
                                }
                                else if (newDimension[z][w][x][y] == '.' && neighbors == 3)
                                {
                                    newDimension[z][w][x][y] = '#';
                                }
                            }
                        }
                    }
                }

                dimension = newDimension.Select(x => x.Select(y => y.Select(t => t.ToArray()).ToArray()).ToArray()).ToArray();
            }

            int count = CountActiveCells(dimension);

            return $"{count}";

            static int CountActiveNeighbors(int z, int w, int x, int y, char[][][][] dimension)
            {
                int count = 0;
                for (int zt = z - 1; zt <= z + 1; zt++)
                {
                    for (int wt = w - 1; wt <= w + 1; wt++)
                    {
                        for (int xt = x - 1; xt <= x + 1; xt++)
                        {
                            for (int yt = y - 1; yt <= y + 1; yt++)
                            {
                                if (dimension[Math.Abs(zt)][Math.Abs(wt)][xt][yt] == '#' && (zt, wt, xt, yt) != (z, w, x, y))
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }

                return count;
            }

            static int CountActiveCells(char[][][][] dimension)
            {
                int count = 0;
                for (int z = -6; z <= 6; z++)
                {
                    for (int w = -6; w <= 6; w++)
                    {
                        for (int i = 0; i < dimension[0][0].Length; i++)
                        {
                            for (int j = 0; j < dimension[0][0][0].Length; j++)
                            {
                                if (dimension[Math.Abs(z)][Math.Abs(w)][i][j] == '#')
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }

                return count;
            }
        }
    }
}
