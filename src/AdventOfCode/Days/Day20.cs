using System.Text;

namespace AdventOfCode.Days;

public class Day20 : Day
{
    public Day20() : base("20")
    {
    }

    public override string Title => "Trench Map";

    public override string ProcessFirst()
    {
        return $"{Process(2)}";
    }

    public override string ProcessSecond()
    {
        return $"{Process(50)}";
    }

    private int Process(int steps)
    {
        string algorithm = Lines.First();

        var points = new Dictionary<(int x, int y), char>();
        int index = 0;
        foreach (string line in Lines.Skip(2))
        {
            for (int i = 0; i < line.Length; i++)
            {
                points[(index, i)] = line[i];
            }
            index++;
        }

        int startingMinX = points.Min(p => p.Key.x) - steps;
        int startingMinY = points.Min(p => p.Key.x) - steps;
        int startingMaxX = points.Max(p => p.Key.x) + steps;
        int startingMaxY = points.Max(p => p.Key.y) + steps;

        char background = '.';
        for (int i = 0; i < steps; i++)
        {
            var newPoints = points.ToDictionary(kv => kv.Key, kv => kv.Value);
            int minX = points.Min(p => p.Key.x) - 1;
            int minY = points.Min(p => p.Key.x) - 1;
            int maxX = points.Max(p => p.Key.x) + 1;
            int maxY = points.Max(p => p.Key.y) + 1;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var binary = new StringBuilder();
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            binary.Append(points.GetValueOrDefault((xx, yy), background) == '#' ? "1" : "0");
                        }
                    }

                    newPoints[(x, y)] = algorithm[Convert.ToInt32(binary.ToString(), 2)];
                }
            }

            points = newPoints;
            background = algorithm[Convert.ToInt32(new string(background == '#' ? '1' : '0', 9), 2)];
        }

        return points
            .Where(c => c.Key.x >= startingMinX && c.Key.x <= startingMaxX && c.Key.y >= startingMinY && c.Key.y <= startingMaxY)
            .Count(c => c.Value == '#');
    }
}
