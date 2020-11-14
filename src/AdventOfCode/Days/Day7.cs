﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day7 : Day
    {
        public override string Title => "Amplification Circuit";

        public override string ProcessFirst()
        {
            int[] program = GetContentAsIntArray(',');
            int maxSignal = 0;

            foreach (int[] permutation in GetPermutations(new int[] { 0, 1, 2, 3, 4 }))
            {
                int signal = Compute(program, permutation);
                maxSignal = Math.Max(maxSignal, signal);
            }

            return maxSignal.ToString();
        }

        public override string ProcessSecond()
        {
            int[] program = GetContentAsIntArray(',');
            int maxSignal = 0;

            foreach (int[] permutation in GetPermutations(new int[] { 5, 6, 7, 8, 9 }))
            {
                int signal = Compute(program, permutation);
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

        private static int Compute(int[] program, int[] settings)
        {
            List<int> outputs = new() { 0 };
            Queue<(int[] program, int p)> amplifiers = new();
            for (int i = 0; i < 5; i++)
            {
                amplifiers.Enqueue((program.ToArray(), 0));
            }

            int loop = 0;
            Queue<int> inputs = new();
            while (amplifiers.Count > 0)
            {
                (int[] amplifier, int p) = amplifiers.Dequeue();

                if (loop < 5)
                {
                    inputs.Enqueue(settings[loop]);
                }

                foreach (int item in outputs)
                {
                    inputs.Enqueue(item);
                }

                (string state, List<int> outputs, int pointer) result = IntcodeInterpreter.Run(amplifier, inputs, p);

                if (result.state == "INPUT_NEEDED")
                {
                    amplifiers.Enqueue((amplifier, result.pointer));
                }

                outputs = result.outputs;

                loop++;
            }

            return outputs[0];
        }
    }
}
