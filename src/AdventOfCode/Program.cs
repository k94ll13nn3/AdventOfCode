using System;
using System.Diagnostics;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args is null || args.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a day number");
                Console.ResetColor();
                return;
            }

            Day day = GetDay(args[0]);

            Console.WriteLine($"Day {args[0]}: {day.Title}{Environment.NewLine}");

            var timer = new Stopwatch();

            timer.Start();

            string firstPart = day.ProcessFirst();
            timer.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"    {firstPart} ");
            Console.ForegroundColor = GetColorForElapsedTime(timer.ElapsedMilliseconds);
            Console.WriteLine($"({timer.ElapsedMilliseconds}ms)");
            Console.ResetColor();

            timer.Restart();

            string secondPart = day.ProcessSecond();
            timer.Stop();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"    {secondPart} ");
            Console.ForegroundColor = GetColorForElapsedTime(timer.ElapsedMilliseconds);
            Console.WriteLine($"({timer.ElapsedMilliseconds}ms)");
            Console.ResetColor();
        }

        private static Day GetDay(string dayNumber)
        {
            return dayNumber switch
            {
                "1" => new Day1(),
                "2" => new Day2(),
                "3" => new Day3(),
                _ => Day.Empty
            };
        }

        private static ConsoleColor GetColorForElapsedTime(long elapsedMilliseconds)
        {
            return elapsedMilliseconds switch
            {
                < 750 => ConsoleColor.Green,
                < 1250 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red,
            };
        }
    }
}
