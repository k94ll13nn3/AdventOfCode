using System;
using System.Diagnostics;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args?.Length < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a day number");
                Console.ResetColor();
                return;
            }

            var type = Type.GetType($"AdventOfCode.Days.Day{args[0]}");
            Day day = type is null ? Day.Empty : (Day)Activator.CreateInstance(type);

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

        private static ConsoleColor GetColorForElapsedTime(long elapsedMilliseconds)
        {
            if (elapsedMilliseconds < 750)
            {
                return ConsoleColor.Green;
            }
            else if (elapsedMilliseconds < 1250)
            {
                return ConsoleColor.Yellow;
            }
            
            return ConsoleColor.Red;
        }
    }
}