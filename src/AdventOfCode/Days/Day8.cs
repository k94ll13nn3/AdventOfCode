using System;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day8 : Day
    {
        public override string Title => "Space Image Format";

        public override string ProcessFirst()
        {
            const int width = 25;
            const int height = 6;

            string input = GetContentAsString();

            int zerosCount = int.MaxValue;
            int onesCount = 0;
            int twosCount = 0;
            for (int i = 0; i < input.Length / (width * height); i++)
            {
                string chunk = input.Substring(i * width * height, width * height);
                int z = chunk.Count(x => x == '0');
                if (z < zerosCount)
                {
                    zerosCount = z;
                    onesCount = chunk.Count(x => x == '1');
                    twosCount = chunk.Count(x => x == '2');
                }
            }

            return (onesCount * twosCount).ToString();
        }

        public override string ProcessSecond()
        {
            const int width = 25;
            const int height = 6;

            string input = GetContentAsString();

            char[,] map = new char[height, width];
            int numberOfLayers = input.Length / (width * height);
            for (int i = 0; i < numberOfLayers; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        if (map[x, y] == '2' || map[x, y] == 0)
                        {
                            map[x, y] = input[(i * width * height) + (x * width) + y];
                        }
                    }
                }
            }

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Console.Write(map[x, y] == '1' ? '\u2588' : ' ');
                }
                Console.WriteLine();
            }

            return 0.ToString();
        }
    }
}
