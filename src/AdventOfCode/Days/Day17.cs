namespace AdventOfCode.Days;

public class Day17 : Day
{
    public Day17() : base("17")
    {
    }

    public override string Title => "Trick Shot";

    public override string ProcessFirst()
    {
        int[] targetArea = Content[13..]
            .Split(new[] { ',', '.', '=', 'x', 'y' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();
        int x1 = targetArea[0];
        int x2 = targetArea[1];
        int y1 = targetArea[2];
        int y2 = targetArea[3];

        int maxYReached = 0;
        for (int y = y1; y <= 1000; y++) // 1000 because it works
        {
            for (int x = 0; x <= x2; x++)
            {
                (bool reached, int maxYReached) result = ComputePath((x, y), (x1, x2, y1, y2));
                if (result.reached)
                {
                    maxYReached = result.maxYReached;
                    break;
                }
            }
        }

        return $"{maxYReached}";
    }

    public override string ProcessSecond()
    {
        int[] targetArea = Content[13..]
          .Split(new[] { ',', '.', '=', 'x', 'y' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
          .Select(int.Parse)
          .ToArray();
        int x1 = targetArea[0];
        int x2 = targetArea[1];
        int y1 = targetArea[2];
        int y2 = targetArea[3];

        int count = 0;
        var l = new List<(int, int)>();
        for (int y = y1; y <= 1000; y++) // 1000 because it works
        {
            for (int x = 0; x <= x2; x++)
            {
                if (ComputePath((x, y), (x1, x2, y1, y2)).reached)
                {
                    count++;
                    l.Add((x, y));
                }
            }
        }

        return $"{count}";
    }

    private static (bool reached, int maxYReached) ComputePath((int x, int y) initialVelocity, (int x1, int x2, int y1, int y2) targetArea)
    {
        (int x, int y) currentPosition = (x: 0, y: 0);
        (int x, int y) velocity = initialVelocity;
        int maxYReached = 0;
        while (currentPosition.x <= targetArea.x2 && currentPosition.y >= targetArea.y1)
        {
            if (currentPosition.x >= targetArea.x1
                && currentPosition.x <= targetArea.x2
                && currentPosition.y >= targetArea.y1
                && currentPosition.y <= targetArea.y2)
            {
                return (true, maxYReached);
            }

            // x won't move and is outside of x1..x2
            if (velocity.x == 0 && currentPosition.x < targetArea.x1)
            {
                return (false, 0);
            }

            // y just go down and is already after y1
            if (velocity.y < 0 && currentPosition.y < targetArea.y1)
            {
                return (false, 0);
            }

            currentPosition = (currentPosition.x + velocity.x, currentPosition.y + velocity.y);
            maxYReached = Math.Max(maxYReached, currentPosition.y);
            if (velocity.x != 0)
            {
                velocity = (velocity.x > 0 ? velocity.x - 1 : velocity.x + 1, velocity.y - 1);
            }
            else
            {
                velocity = (0, velocity.y - 1);
            }
        }

        return (false, 0);
    }
}
