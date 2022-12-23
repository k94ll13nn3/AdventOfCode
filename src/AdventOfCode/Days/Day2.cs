namespace AdventOfCode.Days;

public class Day2 : Day
{
    public Day2() : base("2")
    {
    }

    public override string Title => "Rock Paper Scissors";

    public override string ProcessFirst()
    {
        int score = 0;

        foreach (string line in Lines)
        {
            int result = (line[0], line[2]) switch
            {
                ('A', 'X') => 3,
                ('A', 'Y') => 6,
                ('A', 'Z') => 0,
                ('B', 'X') => 0,
                ('B', 'Y') => 3,
                ('B', 'Z') => 6,
                ('C', 'X') => 6,
                ('C', 'Y') => 0,
                ('C', 'Z') => 3,
                _ => throw new ArgumentException(""),
            };

            score += result + line[2] - 'X' + 1;
        }

        return $"{score}";
    }

    public override string ProcessSecond()
    {
        int score = 0;

        foreach (string line in Lines)
        {
            int result = (line[0], line[2]) switch
            {
                ('A', 'X') => 3,
                ('B', 'X') => 1,
                ('C', 'X') => 2,
                ('A', 'Y') => 1,
                ('B', 'Y') => 2,
                ('C', 'Y') => 3,
                ('A', 'Z') => 2,
                ('B', 'Z') => 3,
                ('C', 'Z') => 1,
                _ => throw new ArgumentException(""),
            };

            score += result + ((line[2] - 'X') * 3);
        }

        return $"{score}";
    }
}
