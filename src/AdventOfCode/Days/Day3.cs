namespace AdventOfCode.Days;

internal sealed class Day3 : Day
{
    public Day3() : base("3")
    {
    }

    public override string Title => "Lobby";

    public override string ProcessFirst()
    {
        return $"{Lines.Sum(line => GetJoltageRec(line, 0, 0, 1))}";
    }

    public override string ProcessSecond()
    {
        return $"{Lines.Sum(line => GetJoltageRec(line, 0, 0, 11))}";
    }

    private static long GetJoltageRec(string line, long acc, int ind, int i)
    {
        if (i is < 0)
        {
            return acc;
        }

        char a = line[ind..^i].Max();
        ind = line.IndexOf(a, ind) + 1;
        return GetJoltageRec(line, (10 * acc) + (a - '0'), ind, i - 1);
    }
}
