using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day8 : Day
    {
        public override string Title => "Memory Maneuver";

        public override string ProcessFirst()
        {
            var input = GetContentAsString().Split(' ').Select(int.Parse);

            return ReadNodes(input).sum.ToString();
        }

        public (int sum, int index) ReadNodes(IEnumerable<int> input)
        {
            var numberOfChildren = input.First();
            var numberOfMetadata = input.Skip(1).First();
            if (numberOfChildren == 0)
            {
                return (input.Skip(2).Take(numberOfMetadata).Sum(), numberOfMetadata + 2);
            }

            var currentSum = 0;
            var currentIndex = 0;
            input = input.Skip(2);
            for (int i = 0; i < numberOfChildren; i++)
            {
                var (sum, index) = ReadNodes(input);
                currentSum += sum;
                currentIndex += index;
                input = input.Skip(index);
            }

            currentSum += input.Take(numberOfMetadata).Sum();

            return (currentSum, currentIndex + numberOfMetadata + 2);
        }

        public override string ProcessSecond()
        {
            var input = GetContentAsString().Split(' ').Select(int.Parse);

            return ReadNodes2(input).sum.ToString();
        }

        public (int sum, int index) ReadNodes2(IEnumerable<int> input)
        {
            var numberOfChildren = input.First();
            var numberOfMetadata = input.Skip(1).First();
            if (numberOfChildren == 0)
            {
                return (input.Skip(2).Take(numberOfMetadata).Sum(), numberOfMetadata + 2);
            }

            var currentIndex = 0;
            input = input.Skip(2);
            var children = new int[numberOfChildren];
            for (int i = 0; i < numberOfChildren; i++)
            {
                var (sum, index) = ReadNodes2(input);
                currentIndex += index;
                children[i] = sum;
                input = input.Skip(index);
            }

            var currentSum = 0;
            foreach (var item in input.Take(numberOfMetadata))
            {
                if (item <= numberOfChildren)
                {
                    currentSum += children[item - 1];
                }
            }

            return (currentSum, currentIndex + numberOfMetadata + 2);
        }
    }
}