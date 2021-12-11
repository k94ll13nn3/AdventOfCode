namespace AdventOfCode.Days;

public class Day11 : Day
{
    public Day11() : base("11")
    {
    }

    public override string Title => "Dumbo Octopus";

    public override string ProcessFirst()
    {
        int[] pad = new string('0', Lines.First().Length + 2).Select(c => int.Parse(c.ToString())).ToArray();
        int[][] map = Lines.Select(l => $"0{l}0".Select(c => int.Parse(c.ToString())).ToArray()).Prepend(pad).Append(pad).ToArray();

        int sum = 0;
        for (int steps = 0; steps < 100; steps++)
        {
            var alreadyFlashed = new List<(int x, int y)>();
            for (int i = 1; i < map.Length - 1; i++)
            {
                for (int j = 1; j < map[0].Length - 1; j++)
                {
                    map[i][j]++;
                }
            }

            var flashables = Flashables(map).Except(alreadyFlashed).ToList();
            alreadyFlashed.AddRange(flashables);
            sum += flashables.Count;
            while (flashables.Count > 0)
            {
                foreach ((int x, int y) in flashables)
                {
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            map[i][j]++;
                        }
                    }
                }

                flashables = Flashables(map).Except(alreadyFlashed).ToList();
                alreadyFlashed.AddRange(flashables);
                sum += flashables.Count;
            }


            foreach ((int x, int y) in alreadyFlashed)
            {
                map[x][y] = 0;
            }
        }

        return $"{sum}";
    }

    public override string ProcessSecond()
    {
        int[] pad = new string('0', Lines.First().Length + 2).Select(c => int.Parse(c.ToString())).ToArray();
        int[][] map = Lines.Select(l => $"0{l}0".Select(c => int.Parse(c.ToString())).ToArray()).Prepend(pad).Append(pad).ToArray();

        int steps = 0;
        int LastNumberOfFlashes = 0;
        while (LastNumberOfFlashes != 100)
        {
            var alreadyFlashed = new List<(int x, int y)>();
            for (int i = 1; i < map.Length - 1; i++)
            {
                for (int j = 1; j < map[0].Length - 1; j++)
                {
                    map[i][j]++;
                }
            }

            var flashables = Flashables(map).Except(alreadyFlashed).ToList();
            alreadyFlashed.AddRange(flashables);
            int sum = flashables.Count;
            while (flashables.Count > 0)
            {
                foreach ((int x, int y) in flashables)
                {
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            map[i][j]++;
                        }
                    }
                }

                flashables = Flashables(map).Except(alreadyFlashed).ToList();
                alreadyFlashed.AddRange(flashables);
                sum += flashables.Count;
            }

            LastNumberOfFlashes = sum;

            foreach ((int x, int y) in alreadyFlashed)
            {
                map[x][y] = 0;
            }
            steps++;
        }

        return $"{steps}";
    }

    private static IEnumerable<(int x, int y)> Flashables(int[][] map)
    {
        for (int i = 1; i < map.Length - 1; i++)
        {
            for (int j = 1; j < map[0].Length - 1; j++)
            {
                if (map[i][j] > 9)
                {
                    yield return (i, j);
                }
            }
        }
    }
}
