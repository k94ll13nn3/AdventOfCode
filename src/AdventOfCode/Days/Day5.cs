namespace AdventOfCode.Days;

public class Day5 : Day
{
    private readonly IEnumerable<int[]> _input;

    public Day5() : base("5")
    {
        _input = Lines.Select(l => l.Split(new char[] { ',', '>', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
    }

    public override string Title => "Hydrothermal Venture";

    public override string ProcessFirst()
    {
        return $"{Process(false)}";
    }

    public override string ProcessSecond()
    {
        return $"{Process(true)}";
    }

    public int Process(bool part2)
    {
        var points = new Dictionary<int, int>();
        foreach (int[] vent in _input)
        {
            if (vent[0] == vent[2])
            {
                for (int i = Math.Min(vent[1], vent[3]); i <= Math.Max(vent[1], vent[3]); i++)
                {
                    Add(vent[0], i);
                }
            }
            else if (vent[1] == vent[3])
            {
                for (int i = Math.Min(vent[0], vent[2]); i <= Math.Max(vent[0], vent[2]); i++)
                {
                    Add(i, vent[1]);
                }
            }
            else if (part2)
            {
                int currentX = vent[0];
                int currentY = vent[1];
                Add(currentX, currentY);
                while (currentX != vent[2] && currentY != vent[3])
                {
                    currentX += vent[0] < vent[2] ? 1 : -1;
                    currentY += vent[1] < vent[3] ? 1 : -1;

                    Add(currentX, currentY);
                }
            }
        }

        return points.Count(kv => kv.Value >= 2);

        void Add(int x, int y)
        {
            points[(x * 100000) + y] = points.GetValueOrDefault((x * 100000) + y, 0) + 1;
        }
    }
}
