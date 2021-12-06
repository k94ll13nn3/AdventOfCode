namespace AdventOfCode.Days;

public class Day6 : Day
{
    public Day6() : base("6")
    {
    }

    public override string Title => "Lanternfish";

    public override string ProcessFirst()
    {
        int indexOfNext = 0;
        int[] state = new int[1000000];
        foreach (int item in Content.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse))
        {
            state[indexOfNext++] = item;
        }

        for (int i = 0; i < 80; i++)
        {
            int count = indexOfNext;
            for (int j = 0; j < count; j++)
            {
                if (state[j] == 0)
                {
                    state[j] = 7;
                    state[indexOfNext++] = 8;
                }
                state[j]--;
            }
        }
        return $"{indexOfNext}";
    }

    public override string ProcessSecond()
    {
        long[] state = new long[9];
        foreach (int item in Content.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse))
        {
            state[item]++;
        }

        for (int i = 0; i < 256; i++)
        {
            long zeros = state[0];
            for (int j = 0; j < state.Length - 1; j++)
            {
                state[j] = state[j + 1];
            }

            state[6] += zeros;
            state[8] = zeros;
        }
        return $"{state.Sum()}";
    }
}
