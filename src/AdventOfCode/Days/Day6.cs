using System.Text;

namespace AdventOfCode.Days;

internal sealed class Day6() : Day("6")
{
    public override string Title => "Trash Compactor";

    public override string ProcessFirst()
    {
        string[][] lines = [.. Lines.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))];
        long count = 0;
        for (int i = 0; i < lines[0].Length; i++)
        {
            string op = lines[lines.Length - 1][i];
            count += lines[..^1].Select(l => long.Parse(l[i])).Aggregate((acc, i) => op == "+" ? i + acc : acc * i);
        }

        return $"{count}";
    }

    public override string ProcessSecond()
    {
        string[] lines = [.. Lines];
        List<long> acc = [];
        long count = 0;
        char lastOp = '0';
        for (int i = 0; i < lines[^1].Length; i++)
        {
            if (lines[^1][i] is '+' or '*')
            {
                count += acc.Count is 0 ? 0 : acc.Aggregate(GetOperator());
                acc = [];
                lastOp = lines[^1][i];
            }

            StringBuilder n = new();
            for (int j = 0; j <= lines.Length - 2; j++)
            {
                n.Append(lines[j][i]);
            }

            acc.Add(Parse(n.ToString()));
        }

        count += acc.Aggregate(GetOperator());

        return $"{count}";

        long Parse(string s)
        {
            string k = s.Trim();
            if (string.IsNullOrWhiteSpace(k))
            {
                return lastOp == '+' ? 0 : 1;
            }

            return long.Parse(k);
        }

        Func<long, long, long> GetOperator()
        {
            return (acc, val) => lastOp == '+' ? val + acc : acc * val;
        }
    }
}
