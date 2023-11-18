namespace AdventOfCode.Days;

public class Day1 : Day
{
    private readonly List<int> _elves;

    public Day1() : base("1")
    {
        _elves = [];
        int count = 0;
        foreach (string line in Lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                _elves.Add(count);
                count = 0;
            }
            else
            {
                count += int.Parse(line);
            }
        }

        _elves.Add(count);
    }

    public override string Title => "Calorie Counting";

    public override string ProcessFirst()
    {
        return $"{_elves.Max(e => e)}";
    }

    public override string ProcessSecond()
    {
        return $"{_elves.OrderByDescending(e => e).Take(3).Sum(e => e)}";
    }
}
