namespace AdventOfCode.Days;

public class Day9 : Day
{
    public Day9() : base("9")
    {
    }

    public override string Title => "Smoke Basin";

    public override string ProcessFirst()
    {
        string pad = new('9', Lines.First().Length + 2);
        string[] map = Lines.Select(l => $"9{l}9").Prepend(pad).Append(pad).ToArray();

        int sum = 0;
        for (int i = 1; i < map.Length - 1; i++)
        {
            for (int j = 1; j < map[0].Length - 1; j++)
            {
                if (map[i - 1][j] > map[i][j] && map[i + 1][j] > map[i][j] && map[i][j - 1] > map[i][j] && map[i][j + 1] > map[i][j])
                {
                    sum += map[i][j] - '0' + 1;
                }
            }
        }

        return $"{sum}";
    }

    public override string ProcessSecond()
    {
        string pad = new('9', Lines.First().Length + 2);
        string[] map = Lines.Select(l => $"9{l}9").Prepend(pad).Append(pad).ToArray();

        var sizes = new List<int>();
        for (int i = 1; i < map.Length - 1; i++)
        {
            for (int j = 1; j < map[0].Length - 1; j++)
            {
                if (map[i - 1][j] > map[i][j] && map[i + 1][j] > map[i][j] && map[i][j - 1] > map[i][j] && map[i][j + 1] > map[i][j])
                {
                    sizes.Add(FindBassin((i, j), map));
                }
            }
        }

        return $"{sizes.OrderByDescending(x => x).Take(3).Aggregate(1, (a, b) => a * b)}";
    }

    private static int FindBassin((int, int) start, string[] map)
    {
        var toVisit = new Queue<(int, int)>(new[] { start });
        var visited = new List<(int, int)>() { start };
        while (toVisit.Count > 0)
        {
            (int x, int y) = toVisit.Dequeue();
            char current = map[x][y];
            if (current != '9')
            {
                (int, int)[] possibles = new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };
                foreach ((int x, int y) possible in possibles.Except(visited))
                {
                    if (map[possible.x][possible.y] != '9' && map[possible.x][possible.y] > current)
                    {
                        toVisit.Enqueue((possible.x, possible.y));
                        visited.Add((possible.x, possible.y));
                    }
                }
            }
        }

        return visited.Count;
    }
}
