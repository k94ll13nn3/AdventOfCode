using System.Text.Json;

namespace AdventOfCode.Days;

public class Day18 : Day
{
    public Day18() : base("18")
    {
    }

    public override string Title => "Snailfish";

    public override string ProcessFirst()
    {
        string current = Lines.First();
        current = ReduceRec(current);
        foreach (string line in Lines.Skip(1))
        {
            current = ReduceRec($"({current},{line})");
        }

        Pair pair = GetPair(current);

        return $"{pair.GetMagnitude()}";
    }

    public override string ProcessSecond()
    {
        int max = 0;
        foreach (string firstLine in Lines)
        {
            foreach (string secondLine in Lines.Where(l => l != firstLine))
            {
                Pair pair = GetPair(ReduceRec($"({firstLine},{secondLine})"));
                max = Math.Max(max, pair.GetMagnitude());
            }
        }

        return $"{max}";
    }
    private string ReduceRec(string line)
    {
        string newLine = line.Replace('[', '(').Replace(']', ')');
        newLine = Explode(newLine);
        string oldLine = line;
        while (newLine != oldLine)
        {
            (oldLine, newLine) = (newLine, Explode(newLine));
        }

        return newLine;
    }

    private string Explode(string line)
    {
        char[]? lineArray = line.ToCharArray();
        int count = 0;
        int index = 0;
        while (count < 5 && index < line.Length - 1)
        {
            if (lineArray[index] == '(')
            {
                count++;
            }

            if (lineArray[index] == ')')
            {
                count--;
            }

            index++;
        }

        if (index == line.Length - 1)
        {
            return Split(line);
        }

        int closingBracket = line.IndexOf(")", index + 1, StringComparison.OrdinalIgnoreCase);

        string inner = line[index..closingBracket];
        int[] parts = inner.Split(',').Select(c => c[0] - '0').ToArray();

        int nextNumber = line.Skip(closingBracket).Select((c, i) => (c, i)).FirstOrDefault(c => c.c >= '0').i;
        if (nextNumber >= 0 && lineArray[nextNumber + closingBracket] is not ('(' or ')' or ','))
        {
            lineArray[nextNumber + closingBracket] = (char)(line[nextNumber + closingBracket] + parts[1]);
        }

        int previousNumber = line.Take(index).Select((c, i) => (c, i)).LastOrDefault(c => c.c >= '0').i;
        if (previousNumber > 0)
        {
            lineArray[previousNumber] = (char)(line[previousNumber] + parts[0]);
        }

        lineArray[index - 1] = '0';

        return string.Join("", lineArray.Where((_, i) => i < index || i > closingBracket).Select(c => c));
    }

    private string Split(string line)
    {
        int nextNumber = line.Select((c, i) => (c, i)).FirstOrDefault(c => c.c > '9').i;
        if (nextNumber > 0)
        {
            int value = line[nextNumber] - '0';

            return line[..nextNumber] + $"({(char)('0' + (value / 2))},{(char)('0' + ((value + 1) / 2))})" + line[(nextNumber + 1)..];
        }

        return line;
    }

    private static Pair GetPair(string value)
    {
        string fixedValue = value
            .Replace("(", "\"Left\":{", StringComparison.OrdinalIgnoreCase)
            .Replace(",", "},\"Right\":{", StringComparison.OrdinalIgnoreCase)
            .Replace(")", "},", StringComparison.OrdinalIgnoreCase);
        for (int i = 0; i < 10; i++)
        {
            fixedValue = fixedValue.Replace($"{i}", $"\"Value\": {i}", StringComparison.OrdinalIgnoreCase);
        }

        return JsonSerializer.Deserialize<Pair>($"{{{fixedValue}}}", new JsonSerializerOptions { AllowTrailingCommas = true })!;
    }

}

public sealed class Pair
{
    public Pair? Left { get; set; }

    public Pair? Right { get; set; }

    public int? Value { get; set; }

    public int GetMagnitude()
    {
        return Value ?? (3 * Left!.GetMagnitude()) + (2 * Right!.GetMagnitude());
    }
}
