namespace AdventOfCode;

public abstract class Day
{
    protected Day(string dayNumber)
    {
        Content = File.ReadAllText($"Inputs/Input{dayNumber}.txt");
        Lines = File.ReadAllLines($"Inputs/Input{dayNumber}.txt");
    }

    public abstract string Title { get; }

    public string Content { get; }

    public IEnumerable<string> Lines { get; }

    public abstract string ProcessFirst();

    public abstract string ProcessSecond();
}
