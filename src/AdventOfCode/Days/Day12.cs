namespace AdventOfCode.Days;

public class Day12 : Day
{
    public Day12() : base("12")
    {
    }

    public override string Title => "Passage Pathing";

    public override string ProcessFirst()
    {
        return $"{Solve(false)}";
    }

    public override string ProcessSecond()
    {
        return $"{Solve(true)}";
    }

    private int Solve(bool part2)
    {
        var paths = new Dictionary<string, List<string>>();
        foreach (string[] line in Lines.Select(l => l.Split('-')))
        {
            // Path
            if (!paths.ContainsKey(line[0]))
            {
                paths[line[0]] = new();
            }

            if (line[1] != "start")
            {
                paths[line[0]].Add(line[1]);
            }

            // Reverse path
            if (!paths.ContainsKey(line[1]))
            {
                paths[line[1]] = new();
            }

            if (line[0] != "start")
            {
                paths[line[1]].Add(line[0]);
            }
        }

        var toVisit = new Stack<(string cave, HashSet<string> visited, bool hasVisitedSmallTwice)>();
        foreach (string cave in paths["start"])
        {
            toVisit.Push((cave, new() { cave }, false));
        }

        int numberOfPaths = 0;
        while (toVisit.Count > 0)
        {
            (string cave, HashSet<string> visited, bool hasVisitedSmallTwice) = toVisit.Pop();
            foreach (string possible in paths[cave])
            {
                if (possible == "end")
                {
                    numberOfPaths++;
                }
                else
                {
                    if (possible[0] is >= 'A' and <= 'Z')
                    {
                        toVisit.Push((possible, visited, hasVisitedSmallTwice));
                    }
                    else if (part2 && (!hasVisitedSmallTwice || !visited.Contains(possible)))
                    {
                        bool newHasVisitedSmallTwice = hasVisitedSmallTwice;
                        if (visited.Contains(possible))
                        {
                            newHasVisitedSmallTwice = true;
                        }

                        var newVisited = visited.ToHashSet();
                        newVisited.Add(possible);
                        toVisit.Push((possible, newVisited, newHasVisitedSmallTwice));
                    }
                    else if (!visited.Contains(possible))
                    {
                        var newVisited = visited.ToHashSet();
                        newVisited.Add(possible);
                        toVisit.Push((possible, newVisited, hasVisitedSmallTwice));
                    }
                }
            }
        }

        return numberOfPaths;
    }
}
