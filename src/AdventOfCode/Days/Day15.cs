namespace AdventOfCode.Days;

public class Day15 : Day
{
    public Day15() : base("15")
    {
    }

    public override string Title => "Chiton";

    public override string ProcessFirst()
    {
        return $"{Process(1)}";
    }

    public override string ProcessSecond()
    {
        return $"{Process(5)}";
    }

    private int Process(int multiplier)
    {
        int[][] map = Lines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();
        Dictionary<(int, int), int> distances = new();
        var queue = new PriorityQueue<(int, int, int), int>();
        queue.Enqueue((0, 0, 0), 1);
        while (queue.Count > 0)
        {
            (int x, int y, int sum) = queue.Dequeue();
            if (x == (multiplier * map.Length) - 1 && y == (multiplier * map[0].Length) - 1)
            {
                return sum;
            }

            foreach ((int i, int j) in new[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) })
            {
                if (i >= 0 && j >= 0 && i < (multiplier * map.Length) && j < (multiplier * map[0].Length) && (distances.GetValueOrDefault((i, j), int.MaxValue) > sum + GetValue(i, j)))
                {
                    distances[(i, j)] = sum + GetValue(i, j);
                    queue.Enqueue((i, j, distances[(i, j)]), distances[(i, j)]);
                }
            }
        }

        return int.MaxValue;

        int GetValue(int xx, int yy)
        {
            int result = map[xx % map.Length][yy % map[0].Length] + (xx / map.Length) + (yy / map[0].Length);
            return result > 9 ? result - 9 : result;
        }
    }
}
