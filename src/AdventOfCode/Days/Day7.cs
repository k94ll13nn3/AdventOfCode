namespace AdventOfCode.Days;

internal sealed class Day7() : Day("7")
{
    public override string Title => "Laboratories";

    public override string ProcessFirst()
    {
        string[] lines = [.. Lines];
        List<int> beams = [lines[0].IndexOf('S')];
        int count = 0;

        for (int i = 1; i < lines.Length - 1; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '^' && beams.Contains(j))
                {
                    beams.Remove(j);
                    if (j >= 1 && !beams.Contains(j - 1))
                    {
                        beams.Add(j - 1);
                    }
                    if (j + 1 < lines[0].Length && !beams.Contains(j + 1))
                    {
                        beams.Add(j + 1);
                    }

                    count++;
                }
            }
        }

        return $"{count}";
    }

    public override string ProcessSecond()
    {
        string[] lines = [.. Lines];
        List<int> beams = [lines[0].IndexOf('S')];
        Dictionary<int, long> timelines = [];
        timelines[lines[0].IndexOf('S')] = 1;

        for (int i = 1; i < lines.Length - 1; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '^' && beams.Contains(j))
                {
                    beams.Remove(j);
                    if (j >= 1)
                    {
                        if (timelines.ContainsKey(j - 1))
                        {
                            timelines[j - 1] += timelines[j];
                        }
                        else
                        {
                            timelines[j - 1] = timelines[j];
                        }
                        if (!beams.Contains(j - 1))
                        {
                            beams.Add(j - 1);
                        }
                    }
                    if (j + 1 < lines[0].Length)
                    {
                        if (timelines.ContainsKey(j + 1))
                        {
                            timelines[j + 1] += timelines[j];
                        }
                        else
                        {
                            timelines[j + 1] = timelines[j];
                        }

                        if (!beams.Contains(j + 1))
                        {
                            beams.Add(j + 1);
                        }
                    }

                    timelines.Remove(j);
                }
            }
        }

        return $"{timelines.Values.Sum()}";
    }
}
