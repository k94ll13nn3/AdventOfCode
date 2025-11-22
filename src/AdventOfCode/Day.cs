namespace AdventOfCode;

internal abstract class Day(string dayNumber)
{
    public abstract string Title { get; }

    public string Number { get; } = dayNumber;

    public string Content { get; } = File.ReadAllText($"Inputs/Input{dayNumber}.txt").Trim();

    public IEnumerable<string> Lines { get; } = File.ReadAllLines($"Inputs/Input{dayNumber}.txt");

    public abstract string ProcessFirst();

    public abstract string ProcessSecond();
}
