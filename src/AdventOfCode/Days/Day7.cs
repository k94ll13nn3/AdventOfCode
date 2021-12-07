namespace AdventOfCode.Days;

public class Day7 : Day
{
    public Day7() : base("7")
    {
    }

    public override string Title => "The Treachery of Whales";

    public override string ProcessFirst()
    {
        var input = Content
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .OrderBy(x => x)
            .ToList();

        int median = input.Count % 2 == 0 ? (input[input.Count / 2] + input[(input.Count / 2) - 1]) / 2 : input[input.Count / 2];

        return $"{input.Sum(v => Math.Abs(v - median))}";
    }

    public override string ProcessSecond()
    {
        var input = Content
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToList();

        int meanFloor = (int)Math.Floor(input.Average());

        int sumCeiling = input.Sum(v =>
        {
            int n = Math.Abs(v - meanFloor);
            return n * (n + 1) / 2;
        });

        int meanCeiling = (int)Math.Ceiling(input.Average());

        int sumFloor = input.Sum(v =>
        {
            int n = Math.Abs(v - meanCeiling);
            return n * (n + 1) / 2;
        });

        return $"{Math.Min(sumFloor, sumCeiling)}";
    }
}
