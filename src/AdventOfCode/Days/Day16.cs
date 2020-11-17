using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day16 : Day
    {
        public override string Title => "Flawed Frequency Transmission";

        public override string ProcessFirst()
        {
            int[] input = GetContent()[0..^2].Select(x => x - 48).ToArray();
            int[] result = Compute(input);

            return string.Join("", result)[..8];
        }

        [SuppressMessage("Major Code Smell", "S1854", Justification = "F*** Sonar")]
        public override string ProcessSecond()
        {
            // Thanks https://www.reddit.com/r/adventofcode/comments/ebf5cy/2019_day_16_part_2_understanding_how_to_come_up/fb4bvw4/.
            int[] input = string.Concat(Enumerable.Repeat(GetContent()[0..^2], 10000)).Select(x => x - 48).ToArray();
            int offset = int.Parse(string.Join("", input[..7]));

            input = input[offset..];
            int[] tempInput = input.ToArray();
            for (int j = 0; j < 100; j++)
            {
                for (int i = tempInput.Length - 2; i >= 0; i--)
                {
                    tempInput[i] += tempInput[i + 1];
                }

                for (int i = 0; i < tempInput.Length; i++)
                {
                    tempInput[i] %= 10;
                }
            }

            return string.Join("", tempInput)[..8];
        }

        private static int[] Compute(int[] input)
        {
            int[] result = new int[input.Length];
            int[] tempInput = input.ToArray();
            int inputLength = input.Length;
            for (int j = 0; j < 100; j++)
            {
                for (int i = 0; i < inputLength; i++)
                {
                    // starts at i because the pattern always has i 0s at start
                    // computation is simplified as multiply by 1 then -1 then 1 then ... every block of i and skipping one block over two
                    int acc = 0;
                    int p = 1;
                    for (int k = i; k < inputLength; k += i + 1)
                    {
                        for (int l = 0; l <= i && k < inputLength; l++)
                        {
                            acc += p * tempInput[k];
                            k++;
                        }

                        p *= -1;
                    }

                    result[i] = Math.Abs(acc % 10);
                }
                tempInput = result.ToArray();
            }

            return result;
        }
    }
}
