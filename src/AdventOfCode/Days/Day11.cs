using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day11 : Day
    {
        public override string Title => "Seating System";

        public override string ProcessFirst()
        {
            char[][] plane = GetLines().Select(x => x.ToCharArray()).ToArray();

            char[][] newPlane = plane.Select(x => x.ToArray()).ToArray();
            int a = 0;
            bool eq = false;
            while (!eq)
            {
                for (int i = 0; i < plane.Length; i++)
                {
                    for (int j = 0; j < plane[0].Length; j++)
                    {
                        int seats = CountOccupiedSeats(i, j, plane);
                        if (plane[i][j] == 'L' && seats == 0)
                        {
                            newPlane[i][j] = '#';
                        }
                        else if (plane[i][j] == '#' && seats >= 4)
                        {
                            newPlane[i][j] = 'L';
                        }
                    }
                }
                eq = ArrayEqual(plane, newPlane);

                plane = newPlane.Select(x => x.ToArray()).ToArray();
                a++;
            }

            return $"{newPlane.Sum(x => x.Count(c => c == '#'))}";
        }

        public override string ProcessSecond()
        {
            char[][] plane = GetLines().Select(x => x.ToCharArray()).ToArray();

            char[][] newPlane = plane.Select(x => x.ToArray()).ToArray();
            int a = 0;
            bool eq = false;
            while (!eq)
            {
                for (int i = 0; i < plane.Length; i++)
                {
                    for (int j = 0; j < plane[0].Length; j++)
                    {
                        int seats = CountOccupiedSeats2(i, j, plane);
                        if (plane[i][j] == 'L' && seats == 0)
                        {
                            newPlane[i][j] = '#';
                        }
                        else if (plane[i][j] == '#' && seats >= 5)
                        {
                            newPlane[i][j] = 'L';
                        }
                    }
                }
                eq = ArrayEqual(plane, newPlane);

                plane = newPlane.Select(x => x.ToArray()).ToArray();
                a++;
            }

            return $"{newPlane.Sum(x => x.Count(c => c == '#'))}";
        }

        private static int CountOccupiedSeats(int i, int j, char[][] plane)
        {
            int count = 0;
            for (int x = Math.Max(0, i - 1); x <= Math.Min(plane.Length - 1, i + 1); x++)
            {
                for (int y = Math.Max(0, j - 1); y <= Math.Min(plane[0].Length - 1, j + 1); y++)
                {
                    if (plane[x][y] == '#' && (x != i || y != j))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int CountOccupiedSeats2(int i, int j, char[][] plane)
        {
            int count = 0;
            for (int x = i - 1; x >= 0; x--)
            {
                if (plane[x][j] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[x][j] == 'L')
                {
                    break;
                }
            }

            for (int x = i + 1; x < plane.Length; x++)
            {
                if (plane[x][j] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[x][j] == 'L')
                {
                    break;
                }
            }

            for (int y = j - 1; y >= 0; y--)
            {
                if (plane[i][y] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i][y] == 'L')
                {
                    break;
                }
            }

            for (int y = j + 1; y < plane[0].Length; y++)
            {
                if (plane[i][y] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i][y] == 'L')
                {
                    break;
                }
            }

            for (int xy = 1; (i - xy) >= 0 && (j - xy) >= 0; xy++)
            {
                if (plane[i - xy][j - xy] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i - xy][j - xy] == 'L')
                {
                    break;
                }
            }

            for (int xy = 1; (i + xy) < plane.Length && (j - xy) >= 0; xy++)
            {
                if (plane[i + xy][j - xy] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i + xy][j - xy] == 'L')
                {
                    break;
                }
            }

            for (int xy = 1; (i - xy) >= 0 && (j + xy) < plane[0].Length; xy++)
            {
                if (plane[i - xy][j + xy] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i - xy][j + xy] == 'L')
                {
                    break;
                }
            }

            for (int xy = 1; (i + xy) < plane.Length && (j + xy) < plane[0].Length; xy++)
            {
                if (plane[i + xy][j + xy] == '#')
                {
                    count++;
                    break;
                }
                else if (plane[i + xy][j + xy] == 'L')
                {
                    break;
                }
            }

            return count;
        }

        private static bool ArrayEqual(char[][] a1, char[][] a2)
        {
            for (int i = 0; i < a1.Length; i++)
            {
                for (int j = 0; j < a1[0].Length; j++)
                {
                    if (a1[i][j] != a2[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
