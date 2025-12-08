namespace AdventOfCode.Days;

internal sealed class Day8() : Day("8")
{
    public override string Title => "Playground";

    public override string ProcessFirst()
    {
        var boxes = Lines.Select(l => l.Split(',').Select(int.Parse).ToList()).ToList();
        List<((int x, int y, int z) left, (int x, int y, int z) right, int distance)> distances = GroupByDistance(boxes, 1000);
        List<List<(int x, int y, int z)>> circuits = [[distances[0].left, distances[0].right]];
        foreach (((int x, int y, int z) left, (int x, int y, int z) right, _) in distances.Skip(1))
        {
            int i = circuits.FindIndex(c => c.Contains(left) || c.Contains(right));
            if (i >= 0)
            {
                circuits[i].AddRange([left, right]);
            }
            else
            {
                circuits.Add([left, right]);
            }
        }


        for (int i = circuits.Count - 1; i >= 1; i--)
        {
            int ij = circuits[..i].FindIndex(c => c.Any(l => circuits[i].Contains(l)));
            if (ij >= 0)
            {
                circuits[ij].AddRange(circuits[i]);
                circuits.RemoveAt(i);
            }

        }

        circuits = [.. circuits.Select(c => c.Distinct().ToList()).OrderByDescending(c => c.Distinct().Count())];

        return $"{circuits.Take(3).Aggregate(1, (acc, e) => acc * e.Count)}";
    }

    public override string ProcessSecond()
    {
        var boxes = Lines.Select(l => l.Split(',').Select(int.Parse).ToList()).ToList();
        List<((int x, int y, int z) left, (int x, int y, int z) right, int distance)> distances = GroupByDistance(boxes);
        List<List<(int x, int y, int z)>> circuits = [[distances[0].left, distances[0].right]];
        foreach (((int x, int y, int z) left, (int x, int y, int z) right, _) in distances.Skip(1))
        {
            int i = circuits.FindIndex(c => c.Contains(left) || c.Contains(right));
            if (i >= 0)
            {
                circuits[i].AddRange([left, right]);
            }
            else
            {
                circuits.Add([left, right]);
            }


            for (int i2 = circuits.Count - 1; i2 >= 1; i2--)
            {
                int ij = circuits[..i2].FindIndex(c => c.Any(l => circuits[i2].Contains(l)));
                if (ij >= 0)
                {
                    circuits[ij].AddRange(circuits[i2]);
                    circuits.RemoveAt(i2);
                }

            }

            if (circuits.Count == 1 && circuits[0].Distinct().Count() == boxes.Count)
            {
                return $"{left.x * right.x}";
            }
        }

        return $"{0}";
    }

    private static List<((int x, int y, int z) left, (int x, int y, int z) right, int distance)> GroupByDistance(List<List<int>> boxes, int? take = null)
    {
        List<((int x, int y, int z) left, (int x, int y, int z) right, int distance)> pairs = [];
        for (int i = 0; i < boxes.Count - 1; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                (int x, int y, int z) left = (boxes[i][0], boxes[i][1], boxes[i][2]);
                (int x, int y, int z) right = (boxes[j][0], boxes[j][1], boxes[j][2]);
                pairs.Add((left, right, GetDistance(left, right)));
            }
        }

        return [.. pairs.OrderBy(c => c.distance).Take(take ?? pairs.Count)];
    }

    private static int GetDistance((int x, int y, int z) left, (int x, int y, int z) right)
    {
        return (int)(Math.Pow(left.x - right.x, 2) + Math.Pow(left.y - right.y, 2) + Math.Pow(left.z - right.z, 2));
    }
}
