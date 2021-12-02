namespace AdventOfCode.Days;

public class Day2 : Day
{
    public Day2() : base("2")
    {
    }

    public override string Title => "Sonar Sweep";

    public override string ProcessFirst()
    {
        int depth = 0;
        int position = 0;
        foreach (string line in Lines)
        {
            int value = line[^1] - '0';
            switch (line[0])
            {
                case 'f':
                    position += value;
                    break;
                case 'd':
                    depth += value;
                    break;
                case 'u':
                    depth -= value;
                    break;
            }
        }

        return $"{depth * position}";
    }

    public override string ProcessSecond()
    {
        int aim = 0;
        int depth = 0;
        int position = 0;
        foreach (string line in Lines)
        {
            int value = line[^1] - '0';
            switch (line[0])
            {
                case 'f':
                    position += value;
                    depth += aim * value;
                    break;
                case 'd':
                    aim += value;
                    break;
                case 'u':
                    aim -= value;
                    break;
            }
        }

        return $"{depth * position}";
    }
}
