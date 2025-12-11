namespace AdventOfCode.Days;

internal sealed class Day10() : Day("10")
{
    public override string Title => "Factory";

    public override string ProcessFirst()
    {
        List<(string lights, List<List<int>> pushes)> inputs = [];
        foreach (string line in Lines)
        {
            string[] parts = line.Split(['[', ']', '{', '}'], StringSplitOptions.RemoveEmptyEntries);
            string wanted = parts[0];
            var pushes = parts[1].Split(['(', ')', ' '], StringSplitOptions.RemoveEmptyEntries).Select(c => c.Split(',').Select(int.Parse).ToList()).ToList();
            inputs.Add((wanted, pushes));
        }

        return $"{inputs.Sum(GetMinCount)}";
    }

    private static int GetMinCount((string lights, List<List<int>> pushes) input)
    {
        string start = new('.', input.lights.Length);

        Stack<(string current, List<int> push, int currentSteps, string[] seens)> state = [];
        foreach (List<int> push in input.pushes)
        {
            state.Push((start, push, 0, []));
        }

        Console.WriteLine("i");

        return GetNextCount(state, input);
    }

    private static int GetNextCount(Stack<(string current, List<int> push, int currentSteps, string[] seens)> state, (string lights, List<List<int>> pushes) input)
    {
        int minSeen = int.MaxValue;
        while (state.Count is > 0)
        {
            (string current, List<int> push, int currentSteps, string[] seens) = state.Pop();

            if (seens.Contains(current) || minSeen <= currentSteps)
            {
                continue;
            }

            char[] newCurrent = current.ToCharArray();
            foreach (int pushButton in push)
            {
                newCurrent[pushButton] = newCurrent[pushButton] == '.' ? '#' : '.';
            }

            string newCurrentStr = new(newCurrent);
            if (newCurrentStr == input.lights)
            {
                minSeen = Math.Min(minSeen, currentSteps + 1);
                continue;
            }

            foreach (List<int> newPush in input.pushes.Where(p => !p.SequenceEqual(push)))
            {
                state.Push((newCurrentStr, newPush, currentSteps + 1, [.. seens, current]));
            }
        }

        return minSeen;
    }

    public override string ProcessSecond()
    {
        return $"{0}";
    }
}
