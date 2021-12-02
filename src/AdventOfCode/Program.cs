using System.Diagnostics;
using AdventOfCode;
using AdventOfCode.Days;

string dayNumber = $"{DateTime.Now.Day}";
if (args?.Length >= 1)
{
    dayNumber = args[0];
}

Day day = GetDay(dayNumber);

Console.WriteLine($"Day {dayNumber}: {day.Title}");
Console.WriteLine();

var timer = new Stopwatch();

timer.Start();

string firstPart = day.ProcessFirst();
timer.Stop();

ColorWrite($"    {firstPart} ", ConsoleColor.DarkGray);
ColorWriteLine($"({timer.ElapsedMilliseconds}ms/{timer.ElapsedTicks} ticks)", GetColorForElapsedTime(timer.ElapsedMilliseconds));

timer.Restart();

string secondPart = day.ProcessSecond();
timer.Stop();

ColorWrite($"    {secondPart} ", ConsoleColor.DarkGray);
ColorWriteLine($"({timer.ElapsedMilliseconds}ms/{timer.ElapsedTicks} ticks)", GetColorForElapsedTime(timer.ElapsedMilliseconds));

static ConsoleColor GetColorForElapsedTime(long elapsedMilliseconds)
{
    return elapsedMilliseconds switch
    {
        < 250 => ConsoleColor.Green,
        < 500 => ConsoleColor.Yellow,
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

static Day GetDay(string dayNumber)
{
    return dayNumber switch
    {
        "1" => new Day1(),
        "2" => new Day2(),
        _ => throw new InvalidOperationException(),
    };
}
