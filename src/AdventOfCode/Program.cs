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
    ["11"] = new Day11(),
    ["12"] = new Day12(),
    ["13"] = new Day13(),
    ["14"] = new Day14(),
    ["15"] = new Day15(),
    ["16"] = new Day16(),
    ["17"] = new Day17(),
};

if (args.Length > 0 && args[0] == "all")
{
    long totalTime = 0L;
    foreach (Day day in days.Values)
    {
        (long part1, long part2) = RunDay(day);
        totalTime += part1 + part2;
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

    if (days.ContainsKey(dayNumber))
    {
        RunDay(days[dayNumber]);
    }
    else
    {
        ColorWrite($"Day {dayNumber} not implemented", ConsoleColor.Red);
    }
}

static (long part1, long part2) RunDay(Day day)
{
    Console.WriteLine($"Day {day.Number}: {day.Title}");
    Console.WriteLine();

    var timer = new Stopwatch();

    timer.Start();

    string firstPart = day.ProcessFirst();
    timer.Stop();

    ColorWrite($"    {firstPart} ", ConsoleColor.DarkGray);
    ColorWriteLine($"({timer.ElapsedMilliseconds}ms/{timer.ElapsedTicks} ticks)", GetColorForElapsedTime(timer.ElapsedMilliseconds));
    long part1Timer = timer.ElapsedMilliseconds;

    timer.Restart();

    string secondPart = day.ProcessSecond();
    timer.Stop();

    ColorWrite($"    {secondPart} ", ConsoleColor.DarkGray);
    ColorWriteLine($"({timer.ElapsedMilliseconds}ms/{timer.ElapsedTicks} ticks)", GetColorForElapsedTime(timer.ElapsedMilliseconds));
    Console.WriteLine();

    return (part1Timer, timer.ElapsedMilliseconds);
}

static ConsoleColor GetColorForElapsedTime(long elapsedMilliseconds)
{
    return elapsedMilliseconds switch
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
