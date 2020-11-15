using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day7 : Day
    {
        public override string Title => "Amplification Circuit";

        public override string ProcessFirst()
        {
            long[] program = GetContentAsLongArray(',');
            long maxSignal = 0;

            foreach (int[] permutation in GetPermutations(new int[] { 0, 1, 2, 3, 4 }))
            {
                long signal = Compute(program, permutation);
                maxSignal = Math.Max(maxSignal, signal);
            }

            return maxSignal.ToString();
        }

        public override string ProcessSecond()
        {
            long[] program = GetContentAsLongArray(',');
            long maxSignal = 0;

            foreach (int[] permutation in GetPermutations(new int[] { 5, 6, 7, 8, 9 }))
            {
                long signal = Compute(program, permutation);
                maxSignal = Math.Max(maxSignal, signal);
            }

            return maxSignal.ToString();
        }

        private static IEnumerable<int[]> GetPermutations(int[] nums)
        {
            var list = new List<int[]>();
            return DoPermute(nums, 0, nums.Length - 1, list);

            static IEnumerable<int[]> DoPermute(int[] nums, int start, int end, IList<int[]> list)
            {
                if (start == end)
                {
                    list.Add(nums.ToArray());
                }
                else
                {
                    for (int i = start; i <= end; i++)
                    {
                        (nums[start], nums[i]) = (nums[i], nums[start]);
                        DoPermute(nums, start + 1, end, list);
                        (nums[start], nums[i]) = (nums[i], nums[start]);
                    }
                }

                return list;
            }
        }

        private static long Compute(long[] program, int[] settings)
        {
            List<long> outputs = new() { 0 };
            Queue<IntcodeInterpreter> amplifiers = new();
            for (int i = 0; i < 5; i++)
            {
                amplifiers.Enqueue(new IntcodeInterpreter(program.ToArray()));
            }

            int loop = 0;
            Queue<long> inputs = new();
            while (amplifiers.Count > 0)
            {
                IntcodeInterpreter amplifier = amplifiers.Dequeue();

                if (loop < 5)
                {
                    inputs.Enqueue(settings[loop]);
                }

                foreach (long item in outputs)
                {
                    inputs.Enqueue(item);
                }

                amplifier.Run(inputs);

                if (amplifier.State == IntcodeInterpreter.InputNeeded)
                {
                    amplifiers.Enqueue(amplifier);
                }

                outputs = amplifier.Outputs;

                loop++;
            }

            return outputs[0];
        }
    }
}
