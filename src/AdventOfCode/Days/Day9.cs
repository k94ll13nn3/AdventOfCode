namespace AdventOfCode.Days;

internal sealed class Day9 : Day
{
    private readonly List<(long x, long y)> _pairs;

    public Day9() : base("9")
    {

        _pairs = [.. Lines.Select(l => l.Split(',').Select(long.Parse).ToArray()).Select(p => (p[0], p[1]))];
    }

    public override string Title => "Movie Theater";

    public override string ProcessFirst()
    {
        long max = 0;
        for (int i = 0; i < _pairs.Count - 1; i++)
        {
            for (int j = i + 1; j < _pairs.Count; j++)
            {
                max = Math.Max(max, GetSurface(_pairs[i], _pairs[j]));
            }
        }

        return $"{max}";
    }

    public override string ProcessSecond()
    {
        // Not correct
        long max = 0;
        for (int i = 0; i < _pairs.Count - 1; i++)
        {
            for (int j = i + 1; j < _pairs.Count; j++)
            {
                if (IsValidZone(_pairs[i], _pairs[j]))
                {
                    Console.WriteLine($"{_pairs[i]}, {_pairs[j]}, {GetSurface(_pairs[i], _pairs[j])}");
                    max = Math.Max(max, GetSurface(_pairs[i], _pairs[j]));
                }
            }
        }

        return $"{max}";
    }

    private bool IsValidZone((long x, long y) a, (long x, long y) b)
    {
        List<(long x, long y)> pointsOnZoneBorder = [];
        long minX = Math.Min(a.x, b.x);
        long maxX = Math.Max(a.x, b.x);
        long minY = Math.Min(a.y, b.y);
        long maxY = Math.Max(a.y, b.y);
        foreach ((long x, long y) point in _pairs.Except([a, b]))
        {
            if ((point.x == minX || point.x == maxX) && minY <= point.y && point.y <= maxY && !pointsOnZoneBorder.Contains(point))
            {
                pointsOnZoneBorder.Add(point);
            }

            if ((point.y == minY || point.y == maxY) && minX <= point.x && point.x <= maxX && !pointsOnZoneBorder.Contains(point))
            {
                pointsOnZoneBorder.Add(point);
            }

            if (minX < point.x && point.x < maxX && minY < point.y && point.y < maxY)
            {
                return false;
            }
        }

        for (int i = 0; i < pointsOnZoneBorder.Count - 1; i++)
        {
            if (pointsOnZoneBorder.Skip(i).Any(p => p.x == pointsOnZoneBorder[i].x || p.y == pointsOnZoneBorder[i].y))
            {
                return false;
            }
        }

        return true;
    }

    private static long GetSurface((long x, long y) a, (long x, long y) b)
    {
        return (1 + Math.Abs(a.x - b.x)) * (1 + Math.Abs(a.y - b.y));
    }
}
