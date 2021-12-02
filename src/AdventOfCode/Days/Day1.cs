namespace AdventOfCode.Days;

public class Day1 : Day
{
    public Day1() : base("1")
    {
    }

    public override string Title => "Sonar Sweep";

    public override string ProcessFirst()
    {
        var measures = Lines.Select(int.Parse).ToList();

        int count = 0;
        for (int i = 1; i < measures.Count; i++)
        {
            if (measures[i] > measures[i - 1])
            {
                count++;
            }
        }

        return $"{count}";
    }

    public override string ProcessSecond()
    {
        var measures = Lines.Select(int.Parse).ToList();

        int count = 0;
        for (int i = 3; i < measures.Count; i++)
        {
            // A + B + C > B + C + D <=> A > D
            if (measures[i] > measures[i - 3])
            {
                count++;
            }
        }

        return $"{count}";
    }
}
