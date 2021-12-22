namespace AdventOfCode.Days;
public class Day22 : Day
{
    public Day22() : base("22")
    {
    }

    public override string Title => "Reactor Reboot";

    public override string ProcessFirst()
    {
        var cubes = new HashSet<(int, int, int)>();
        foreach (string? line in Lines)
        {
            string[] parts = line.Split(new[] { ' ', 'x', '=', 'y', 'z', '.', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int x = Math.Max(-50, int.Parse(parts[1])); x <= Math.Min(50, int.Parse(parts[2])); x++)
            {
                for (int y = Math.Max(-50, int.Parse(parts[3])); y <= Math.Min(50, int.Parse(parts[4])); y++)
                {
                    for (int z = Math.Max(-50, int.Parse(parts[5])); z <= Math.Min(50, int.Parse(parts[6])); z++)
                    {
                        (int x, int y, int z) cube = (x, y, z);
                        if (parts[0] == "on")
                        {
                            if (!cubes.Contains(cube))
                            {
                                cubes.Add(cube);
                            }
                        }
                        else
                        {
                            if (cubes.Contains(cube))
                            {
                                cubes.Remove(cube);
                            }
                        }
                    }
                }
            }
        }

        return $"{cubes.Count}";
    }

    public override string ProcessSecond()
    {
        var cubes = new List<Cuboid>();
        foreach (string line in Lines)
        {
            string[] parts = line.Split(new[] { ' ', 'x', '=', 'y', 'z', '.', ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            (int xx, int xy) = (int.Parse(parts[1]), int.Parse(parts[2]));
            (int yx, int yy) = (int.Parse(parts[3]), int.Parse(parts[4]));
            (int zx, int zy) = (int.Parse(parts[5]), int.Parse(parts[6]));
            var cube = new Cuboid(xx, xy, yx, yy, zx, zy, parts[0] == "on");
            foreach (Cuboid item in cubes.ToList())
            {
                if (Intersect(item, cube) is Cuboid newCube)
                {
                    cubes.Add(newCube);
                }
            }

            if (cube.on)
            {
                cubes.Add(cube);
            }
        }

        return $"{cubes.Aggregate(0L, (acc, c) => acc + Count(c))}";
    }

    private static Cuboid? Intersect(Cuboid first, Cuboid second)
    {
        if (first.xx > second.xy || first.xy < second.xx || first.yx > second.yy || first.yy < second.yx || first.zx > second.zy || first.zy < second.zx)
        {
            return null;
        }

        bool on;
        if (first.on && second.on)
        {
            on = false;
        }
        else if (!first.on && !second.on)
        {
            on = true;
        }
        else
        {
            on = second.on;
        }

        return new Cuboid(
            Math.Max(first.xx, second.xx), Math.Min(first.xy, second.xy),
            Math.Max(first.yx, second.yx), Math.Min(first.yy, second.yy),
            Math.Max(first.zx, second.zx), Math.Min(first.zy, second.zy),
            on);
    }

    private static long Count(Cuboid c)
    {
        return (c.xy - c.xx + 1L) * (c.yy - c.yx + 1L) * (c.zy - c.zx + 1L) * (c.on ? 1 : -1);
    }

    private sealed record Cuboid(int xx, int xy, int yx, int yy, int zx, int zy, bool on);
}
