// stylecop.header

using System;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal class Program
    {
        private static void Main()
        {
            var day = new Day13();

            Console.WriteLine($"First problem: {day.ProcessFirst()}");
            Console.WriteLine($"Second problem: {day.ProcessSecond()}");
            Console.ReadLine();
        }
    }
}