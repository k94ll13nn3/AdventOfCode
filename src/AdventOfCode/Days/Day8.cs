namespace AdventOfCode.Days;

public class Day8 : Day
{
    public Day8() : base("8")
    {
    }

    public override string Title => "Seven Segment Search";

    public override string ProcessFirst()
    {
        return $"{Lines.Select(l => l.Split('|', StringSplitOptions.TrimEntries)[1].Split(' ').Count(s => s.Length is 2 or 3 or 4 or 7)).Sum()}";
    }

    public override string ProcessSecond()
    {
        int sum = 0;
        foreach (string[] line in Lines.Select(l => l.Split(new[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries)))
        {
            // In order : 1, 7, 4, (2, 3, 5), (0, 6, 9), 8
            List<string> segments = line[..^4].OrderBy(s => s.Length).ToList();

            // 7 except 1 => a
            // 3 intersect 5 intersect 2 intersect 4 => d
            // 3 intersect 5 intersect 2 => g (knowing a, d)
            // 4 except 1 => b (knowing d)
            // 5 (length 5 and containing b) => f (knowing a, b, d, g)
            // 1 => c (knowing f)
            // 8 => e (knowing a, b, c, d, f, g)
            var map = new Dictionary<char, char>();
            IEnumerable<char> seven = segments[1];
            IEnumerable<char> one = segments[0];
            IEnumerable<char> four = segments[2];
            IEnumerable<char> eight = segments[^1];
            IEnumerable<char> twoThreeFive = segments[3].Intersect(segments[4]).Intersect(segments[5]);

            map['a'] = seven.Except(one).Single();
            map['d'] = twoThreeFive.Intersect(four).Single();
            map['g'] = twoThreeFive.Single(c => c != map['a'] && c != map['d']);
            map['b'] = four.Except(one).Single(c => c != map['d']);
            map['f'] = segments.First(s => s.Length == 5 && s.Any(c => c == map['b'])).Single(c => c != map['a'] && c != map['g'] && c != map['b'] && c != map['d']);
            map['c'] = one.Single(c => c != map['f']);
            map['e'] = eight.Single(c => c != map['a'] && c != map['b'] && c != map['c'] && c != map['d'] && c != map['f'] && c != map['g']);

            var reverseMap = map.ToDictionary(kv => kv.Value, kv => kv.Key);

            string number = string.Concat(line[^4..].Select(digits => PatternToDigit(string.Concat(digits.Select(c => reverseMap[c]).OrderBy(c => c)))));

            sum += int.Parse(number);
        }

        return $"{sum}";
    }

    private static char PatternToDigit(string pattern)
    {
        return pattern switch
        {
            "abcefg" => '0',
            "cf" => '1',
            "acdeg" => '2',
            "acdfg" => '3',
            "bcdf" => '4',
            "abdfg" => '5',
            "abdefg" => '6',
            "acf" => '7',
            "abcdefg" => '8',
            "abcdfg" => '9',
            _ => '\0',
        };
    }
}
