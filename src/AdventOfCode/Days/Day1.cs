namespace AdventOfCode.Days;

public class Day1 : Day
{
    private readonly List<int> _measures;

    public Day1() : base("1")
    {
        _measures = Lines.Select(int.Parse).ToList();
    }

    public override string Title => "Sonar Sweep";

    public override string ProcessFirst()
    {
        int count = 0;
        for (int i = 1; i < _measures.Count; i++)
        {
            if (_measures[i] > _measures[i - 1])
            {
                count++;
            }
        }

        return $"{count}";
    }

    public override string ProcessSecond()
    {
        int count = 0;
        for (int i = 3; i < _measures.Count; i++)
        {
            // A + B + C > B + C + D <=> A > D
            if (_measures[i] > _measures[i - 3])
            {
                count++;
            }
        }

        return $"{count}";
    }
}
