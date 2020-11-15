using System;
using System.Collections.Generic;
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
                ColorWriteLine("Please enter a day number", ConsoleColor.Red);
                return;
            }

            Day day = GetDay(args[0]);

            Console.WriteLine($"Day {args[0]}: {day.Title}");
            Console.WriteLine();

            var timer = new Stopwatch();

            timer.Start();

            string firstPart = day.ProcessFirst();
            timer.Stop();

            ColorWrite($"    {firstPart} ", ConsoleColor.DarkGray);
            ColorWriteLine($"({timer.ElapsedMilliseconds}ms)", GetColorForElapsedTime(timer.ElapsedMilliseconds));

            timer.Restart();

            string secondPart = day.ProcessSecond();
            timer.Stop();

            ColorWrite($"    {secondPart} ", ConsoleColor.DarkGray);
            ColorWriteLine($"({timer.ElapsedMilliseconds}ms)", GetColorForElapsedTime(timer.ElapsedMilliseconds));

            Console.WriteLine();
            Console.WriteLine("Checking Intcode...");
            foreach ((Day intcodeDay, string firstAnswer, string secondAnswer) in GetIntcodeDays())
            {
                ColorWrite($"{intcodeDay.GetType()} ({intcodeDay.Title}): ", ConsoleColor.DarkGray);
                if (intcodeDay.ProcessFirst() == firstAnswer)
                {
                    ColorWrite("OK", ConsoleColor.Green);
                }
                else
                {
                    ColorWrite("KO", ConsoleColor.Red);
                }

                ColorWrite("/", ConsoleColor.DarkGray);

                if (intcodeDay.ProcessSecond() == secondAnswer)
                {
                    ColorWrite("OK", ConsoleColor.Green);
                }
                else
                {
                    ColorWrite("KO", ConsoleColor.Red);
                }

                Console.WriteLine();
            }
        }

        private static Day GetDay(string dayNumber)
        {
            return dayNumber switch
            {
                "1" => new Day1(),
                "2" => new Day2(),
                "3" => new Day3(),
                "4" => new Day4(),
                "5" => new Day5(),
                "6" => new Day6(),
                "7" => new Day7(),
                "8" => new Day8(),
                "9" => new Day9(),
                _ => Day.Empty
            };
        }

        private static List<(Day intcodeDay, string firstAnswer, string secondAnswer)> GetIntcodeDays()
        {
            return new()
            {
                (new Day2(), "4138687", "6635"),
                (new Day5(), "2845163", "9436229"),
                (new Day7(), "18812", "25534964"),
                (new Day9(), "2457252183", "70634"),
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

        private static void ColorWrite(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        private static void ColorWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
