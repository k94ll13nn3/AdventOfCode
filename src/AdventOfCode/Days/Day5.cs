namespace AdventOfCode.Days;

internal sealed class Day5() : Day("5")
{
    public override string Title => "Printing Department";

    public override string ProcessFirst()
    {
        string[] lines = [.. Lines];
        (int i, List<(long min, long max)> ranges) = GetRanges(lines);

        i++;
        int count = 0;
        while (i < lines.Length)
        {
            long id = long.Parse(lines[i]);
            if (ranges.Any(r => r.min <= id && id <= r.max))
            {
                count++;
            }

            i++;
        }

        return $"{count}";
    }

    public override string ProcessSecond()
    {
        string[] lines = [.. Lines];
        List<(long min, long max)> ranges = GetRanges(lines).ranges;

        int i = 0;
        ranges = [.. ranges.OrderBy(r => r.min).ThenBy(r => r.max)];
        while (i < ranges.Count - 1)
        {
            if (ranges[i].min <= ranges[i + 1].min && ranges[i + 1].min <= ranges[i].max)
            {
                (long min, long) newRange = (ranges[i].min, Math.Max(ranges[i + 1].max, ranges[i].max));
                ranges.RemoveAt(i);
                ranges.RemoveAt(i);
                ranges.Insert(i, newRange);
            }
            else
            {
                i++;
            }
        }

        return $"{ranges.Sum(r => r.max + 1 - r.min)}";
    }

    private static (int i, List<(long min, long max)> ranges) GetRanges(string[] lines)
    {
        List<(long min, long max)> ranges = [];
        int i = 0;
        while (!string.IsNullOrWhiteSpace(lines[i]))
        {
            var parts = lines[i].Split('-').Select(long.Parse).ToList();

            ranges.Add((parts[0], parts[1]));
            i++;
        }

        return (i, ranges);
    }
}
