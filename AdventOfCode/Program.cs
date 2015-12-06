using AdventOfCode.Days;
using System;

namespace AdventOfCode
{
    internal class Program
    {
        private static void Main()
        {
            var day = new Day6();

            Console.WriteLine($"First problem: {day.ProcessFirst()}");
            Console.WriteLine($"Second problem: {day.ProcessSecond()}");
            Console.ReadLine();
        }
    }
}