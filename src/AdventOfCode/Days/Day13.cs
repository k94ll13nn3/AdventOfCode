namespace AdventOfCode.Days;

public class Day13 : Day
{
    public Day13() : base("13")
    {
    }

    public override string Title => "Transparent Origami";

    public override string ProcessFirst()
    {
        List<(int x, int y)> points = Lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(line =>
        {
            string[] parts = line.Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        })
        .ToList();

        IEnumerable<(char direction, int position)> instructions = Lines
            .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => (line[11], int.Parse(line[13..])));

        foreach ((char direction, int position) in instructions)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (direction == 'x')
                {
                    if (points[i].x > position)
                    {
                        points[i] = ((2 * position) - points[i].x, points[i].y);
                    }
                }
                else
                {
                    if (points[i].y > position)
                    {
                        points[i] = (points[i].x, (2 * position) - points[i].y);
                    }
                }
            }

            break;
        }

        return $"{points.Distinct().Count()}";
    }

    public override string ProcessSecond()
    {
        List<(int x, int y)> points = Lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(line =>
        {
            string[] parts = line.Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        })
        .ToList();

        IEnumerable<(char direction, int position)> instructions = Lines
            .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => (line[11], int.Parse(line[13..])));

        foreach ((char direction, int position) in instructions)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (direction == 'x')
                {
                    if (points[i].x > position)
                    {
                        points[i] = ((2 * position) - points[i].x, points[i].y);
                    }
                }
                else
                {
                    if (points[i].y > position)
                    {
                        points[i] = (points[i].x, (2 * position) - points[i].y);
                    }
                }
            }

            points = points.Distinct().ToList();
        }

        int maxX = points.Max(p => p.x) + 1;
        int maxY = points.Max(p => p.y) + 1;
        char[][] map = new char[maxX][];
        for (int i = 0; i < maxX; i++)
        {
            map[i] = new char[maxY];
            for (int j = 0; j < maxY; j++)
            {
                map[i][j] = ' ';
            }
        }

        foreach ((int x, int y) in points)
        {
            map[x][y] = '█';
        }

        for (int j = 0; j < maxY; j++)
        {
            for (int i = 0; i < maxX; i++)
            {
                Console.Write(map[i][j]);
            }

            Console.WriteLine();
        }

        return "-";
    }
}
