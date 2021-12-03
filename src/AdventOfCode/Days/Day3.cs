namespace AdventOfCode.Days;

public class Day3 : Day
{
    public Day3() : base("3")
    {
    }

    public override string Title => "Binary Diagnostic";

    public override string ProcessFirst()
    {
        string occurences = GetOccurences(Lines);

        int gamma = Convert.ToInt32(occurences, 2);
        int epsilon = ~gamma & 0b111111111111; // won't work for inputs that are not composed of 12 bits
        return $"{gamma * epsilon}";
    }

    public override string ProcessSecond()
    {
        int oxygen = Convert.ToInt32(FindInList(Lines, 0, true).First(), 2);
        int co2 = Convert.ToInt32(FindInList(Lines, 0, false).First(), 2);
        return $"{oxygen * co2}";
    }

    private IEnumerable<string> FindInList(IEnumerable<string> input, int index, bool equal)
    {
        if (input.Count() == 1 || index >= input.First().Length)
        {
            return input;
        }

        string occurences = GetOccurences(input);

        if (equal)
        {
            return FindInList(input.Where(line => line[index] == occurences[index]), index + 1, equal);
        }
        else
        {
            return FindInList(input.Where(line => line[index] != occurences[index]), index + 1, equal);
        }
    }

    private static string GetOccurences(IEnumerable<string> input)
    {
        var occurences = new List<int>(Enumerable.Repeat(0, input.First().Length));
        foreach (string line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                occurences[i] += line[i] == '1' ? 1 : -1;
            }
        }

        return new string(occurences.Select(c => c >= 0 ? '1' : '0').ToArray());
    }
}
