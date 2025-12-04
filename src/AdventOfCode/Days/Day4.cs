namespace AdventOfCode.Days;

internal sealed class Day4 : Day
{
    public Day4() : base("4")
    {
    }

    public override string Title => "Printing Department";

    public override string ProcessFirst()
    {
        char[][] lines = [.. Lines.Select(s => s.ToCharArray())];

        return $"{GetAccessibleRolls(lines).Count}";
    }

    public override string ProcessSecond()
    {
        char[][] lines = [.. Lines.Select(s => s.ToCharArray())];

        List<(int x, int y)> rolls = GetAccessibleRolls(lines);
        int count = rolls.Count;

        while (rolls.Count > 0)
        {
            foreach ((int x, int y) in rolls)
            {
                lines[x][y] = '.';
            }

            rolls = GetAccessibleRolls(lines);
            count += rolls.Count;
        }


        return $"{count}";
    }

    private static List<(int x, int y)> GetAccessibleRolls(char[][] lines)
    {
        List<(int x, int y)> rolls = [];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] != '@')
                {
                    continue;
                }

                int adjacents = 0;
                for (int ii = i - 1; ii <= i + 1; ii++)
                {
                    for (int jj = j - 1; jj <= j + 1; jj++)
                    {
                        if ((ii != i || jj != j) && ii >= 0 && ii < lines.Length && jj >= 0 && jj < lines[0].Length && lines[ii][jj] is '@')
                        {
                            adjacents++;
                        }
                    }
                }

                if (adjacents < 4)
                {
                    rolls.Add((i, j));
                }
            }
        }

        return rolls;
    }
}
