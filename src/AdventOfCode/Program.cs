using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Days;

namespace AdventOfCode
{
    internal static partial class Program
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

            if (!Debugger.IsAttached && GetCoupledDays().Any(x => x.coupledDay.GetType() == day.GetType()))
            {
                Console.WriteLine();
                Console.WriteLine("Checking coupled days...");
                foreach ((Day coupledDay, string firstAnswer, string secondAnswer) in GetCoupledDays())
                {
                    ColorWrite($"{coupledDay.GetType().ToString().Split('.')[^1]} ({coupledDay.Title}): ", ConsoleColor.DarkGray);
                    if (coupledDay.ProcessFirst() == firstAnswer)
                    {
                        ColorWrite("OK", ConsoleColor.Green);
                    }
                    else
                    {
                        ColorWrite("KO", ConsoleColor.Red);
                    }

                    ColorWrite("/", ConsoleColor.DarkGray);

                    if (coupledDay.ProcessSecond() == secondAnswer)
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
        }

        private static ConsoleColor GetColorForElapsedTime(long elapsedMilliseconds)
        {
            return elapsedMilliseconds switch
            {
                < 250 => ConsoleColor.Green,
                < 500 => ConsoleColor.Yellow,
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
