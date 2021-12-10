namespace AdventOfCode.Days;

public class Day10 : Day
{
    public Day10() : base("10")
    {
    }

    public override string Title => "Syntax Scoring";

    public override string ProcessFirst()
    {
        int sum = 0;
        foreach (string line in Lines)
        {
            var expectedClosing = new Stack<char>();
            foreach (char character in line)
            {
                if (expectedClosing.Count > 0 && (character is ']' or ')' or '>' or '}'))
                {
                    char expected = expectedClosing.Pop();
                    if (expected != character)
                    {
                        sum += character switch
                        {
                            ')' => 3,
                            ']' => 57,
                            '}' => 1197,
                            '>' => 25137,
                            _ => 0,
                        };
                        break;
                    }
                }

                if (character is '[')
                {
                    expectedClosing.Push(']');
                }

                if (character is '(')
                {
                    expectedClosing.Push(')');
                }
                if (character is '<')
                {
                    expectedClosing.Push('>');
                }
                if (character is '{')
                {
                    expectedClosing.Push('}');
                }
            }
        }

        return $"{sum}";
    }

    public override string ProcessSecond()
    {
        var scores = new List<long>();
        foreach (string line in Lines)
        {
            bool stopped = false;
            var expectedClosing = new Stack<char>();
            foreach (char character in line)
            {
                if (expectedClosing.Count > 0 && (character is ']' or ')' or '>' or '}'))
                {
                    char expected = expectedClosing.Pop();
                    if (expected != character)
                    {
                        stopped = true;
                        break;
                    }
                }

                if (character is '[')
                {
                    expectedClosing.Push(']');
                }

                if (character is '(')
                {
                    expectedClosing.Push(')');
                }
                if (character is '<')
                {
                    expectedClosing.Push('>');
                }
                if (character is '{')
                {
                    expectedClosing.Push('}');
                }
            }

            if (!stopped)
            {
                long innerSum = 0;

                while (expectedClosing.Count > 0)
                {
                    char current = expectedClosing.Pop();
                    innerSum *= 5;
                    innerSum += current switch
                    {
                        ')' => 1,
                        ']' => 2,
                        '}' => 3,
                        '>' => 4,
                        _ => 0,
                    };
                }

                scores.Add(innerSum);
            }
        }

        return $"{scores.OrderBy(c => c).ToList()[scores.Count / 2]}";
    }
}
