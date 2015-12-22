// stylecop.header
using System;
using AdventOfCode.Days;
using System.Diagnostics;

namespace AdventOfCode
{
    internal class Program
    {
        private static void Main()
        {
            var day = new Day22();
            var timer = new Stopwatch();

            timer.Start();
            var firstPart = day.ProcessFirst();
            timer.Stop();
            Console.WriteLine($"First problem: {firstPart} in {timer.ElapsedMilliseconds / 1000.0}s");

            timer.Start();
            var secondPart = day.ProcessSecond();
            timer.Stop();
            Console.WriteLine($"Second problem: {secondPart} in {timer.ElapsedMilliseconds / 1000.0}s");

            Console.ReadLine();
        }
    }
}