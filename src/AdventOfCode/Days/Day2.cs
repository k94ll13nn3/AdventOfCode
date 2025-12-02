namespace AdventOfCode.Days;

internal sealed class Day2 : Day
{
    private readonly List<long> _ids;

    public Day2() : base("2")
    {
        _ids = [];
        foreach (string line in Content.Split(','))
        {
            var parts = line.Split('-').Select(long.Parse).ToList();

            _ids.AddRange(Enumerable.Sequence(parts[0], parts[1], 1));
        }
    }

    public override string Title => "Gift Shop";

    public override string ProcessFirst()
    {
        List<long> invalids = [];
        foreach (long id in _ids)
        {
            string s = id.ToString();
            if (s.Length % 2 == 0 && s[..(s.Length / 2)] == s[(s.Length / 2)..])
            {
                invalids.Add(id);
            }
        }

        return $"{invalids.Sum()}";
    }

    public override string ProcessSecond()
    {
        List<long> invalids = [];
        foreach (long id in _ids)
        {
            string s = id.ToString();
            for (int i = 1; i <= s.Length / 2; i++)
            {
                if (s.Length % i == 0 && s == string.Concat(Enumerable.Repeat(s[..i], s.Length / i)))
                {
                    invalids.Add(id);
                    break;
                }
            }
        }

        return $"{invalids.Sum()}";
    }
}
