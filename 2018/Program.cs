using System;
using System.Diagnostics;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var type = Type.GetType($"AdventOfCode.Days.Day{args[0]}");
            Day day = type is null ? Day.Empty : (Day)Activator.CreateInstance(type);

            var timer = new Stopwatch();

            timer.Start();
            string firstPart = day.ProcessFirst();
            timer.Stop();
            Console.WriteLine($"First problem: {firstPart} in {timer.Elapsed.Seconds}s");

            timer.Restart();
            string secondPart = day.ProcessSecond();
            timer.Stop();
            Console.WriteLine($"Second problem: {secondPart} in {timer.Elapsed.Seconds}s");
        }
    }
}