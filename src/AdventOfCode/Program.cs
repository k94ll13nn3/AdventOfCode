using System.Diagnostics;
using AdventOfCode;
using AdventOfCode.Days;

var days = new Dictionary<string, Day>
{
    ["1"] = new Day1(),
    ["2"] = new Day2(),
    ["3"] = new Day3(),
    ["4"] = new Day4(),
    ["5"] = new Day5(),
    ["6"] = new Day6(),
    ["7"] = new Day7(),
    ["8"] = new Day8(),
    ["9"] = new Day9(),
    ["10"] = new Day10(),
};

if (args.Length > 0 && args[0] == "all")
{
    long totalTime = 0L;
    foreach (Day day in days.Values)
    {
        (double part1, double part2) = RunDay(day);
        totalTime += (long)(part1 + part2);
    }

    ColorWriteLine($"Total time: {totalTime}ms", ConsoleColor.DarkCyan);
}
else
{
    string dayNumber = $"{DateTime.Now.Day}";
    if (args?.Length >= 1)
    {
        dayNumber = args[0];
    }

    if (days.TryGetValue(dayNumber, out Day? value))
    {
        RunDay(value);
    }
    else
    {
        ColorWrite($"Day {dayNumber} not implemented", ConsoleColor.Red);
    }
}

static (double part1, double part2) RunDay(Day day)
{
    Console.WriteLine($"Day {day.Number}: {day.Title}");
    Console.WriteLine();

    var timer = new Stopwatch();

    timer.Start();

    string firstPart = day.ProcessFirst();
    timer.Stop();

    ColorWrite($"    {firstPart} ", ConsoleColor.DarkGray);
    ColorWriteLine($"({(long)timer.Elapsed.TotalMilliseconds}ms/{timer.Elapsed.TotalMicroseconds}µs)", GetColorForElapsedTime(timer.Elapsed.TotalMilliseconds));
    double part1Timer = timer.Elapsed.TotalMilliseconds;

    timer.Restart();

    string secondPart = day.ProcessSecond();
    timer.Stop();

    ColorWrite($"    {secondPart} ", ConsoleColor.DarkGray);
    ColorWriteLine($"({(long)timer.Elapsed.TotalMilliseconds}ms/{timer.Elapsed.TotalMicroseconds}µs)", GetColorForElapsedTime(timer.Elapsed.TotalMilliseconds));
    Console.WriteLine();

    return (part1Timer, timer.Elapsed.TotalMilliseconds);
}

static ConsoleColor GetColorForElapsedTime(double totalMilliseconds)
{
    return totalMilliseconds switch
    {
        < 25 => ConsoleColor.Green,
        < 50 => ConsoleColor.Yellow,
        _ => ConsoleColor.Red,
    };
}

static void ColorWrite(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

static void ColorWriteLine(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}
