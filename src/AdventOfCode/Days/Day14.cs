namespace AdventOfCode.Days;

public class Day14 : Day
{
    public Day14() : base("14")
    {
    }

    public override string Title => "Extended Polymerization";

    public override string ProcessFirst()
    {
        return Process(10);
    }

    public override string ProcessSecond()
    {
        return Process(40);
    }

    private string Process(int steps)
    {
        string template = Lines.First();

        var insertions = Lines
            .Skip(2)
            .ToDictionary(line => line[0..2], line => line[6]);

        var occurences = new Dictionary<string, long>();
        for (int i = 0; i < template.Length - 1; i++)
        {
            occurences[template[i..(i + 2)]] = occurences.GetValueOrDefault(template[i..(i + 2)], 0) + 1;
        }

        for (int i = 0; i < steps; i++)
        {
            var newOccurences = new Dictionary<string, long>();
            foreach (KeyValuePair<string, long> occurence in occurences)
            {
                char pair = insertions[occurence.Key];
                newOccurences[$"{occurence.Key[0]}{pair}"] = newOccurences.GetValueOrDefault($"{occurence.Key[0]}{pair}", 0) + occurence.Value;
                newOccurences[$"{pair}{occurence.Key[1]}"] = newOccurences.GetValueOrDefault($"{pair}{occurence.Key[1]}", 0) + occurence.Value;
            }

            occurences = newOccurences;
        }

        var charMap = new Dictionary<char, long>();
        foreach (KeyValuePair<string, long> occurence in occurences)
        {
            charMap[occurence.Key[0]] = charMap.GetValueOrDefault(occurence.Key[0], 0) + occurence.Value;
        }

        charMap[template[^1]]++;

        var occurencesSorted = charMap.OrderByDescending(g => g.Value).ToList();
        return $"{occurencesSorted[0].Value - occurencesSorted[^1].Value}";
    }
}
