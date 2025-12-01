namespace AdventOfCode.Days;

internal sealed class Day1 : Day
{
    private readonly List<int> _rotations;

    public Day1() : base("1")
    {
        _rotations = [50];
        foreach (string line in Lines)
        {
            int rotation = int.Parse(line[1..]);
            if (line[0] is 'L')
            {
                rotation *= -1;
            }

            _rotations.Add(rotation);
        }
    }

    public override string Title => "Secret Entrance";

    public override string ProcessFirst()
    {
        int result = 0;
        int acc = 0;
        foreach (int rotation in _rotations)
        {
            acc = (acc + rotation) % 100;
            if (acc < 0)
            {
                acc += 100;
            }

            if (acc is 0)
            {
                result++;
            }
        }

        return $"{result}";
    }

    public override string ProcessSecond()
    {
        int result = 0;
        int acc = 0;
        foreach (int rotation in _rotations)
        {
            int oldAcc = acc;
            acc = (acc + rotation) % 100;
            if (acc < 0)
            {
                acc += 100;
            }

            // Old value must not be 0 because when starting from 0 the only possible way to see 0
            // again is if the rotation is >= 100 and that case is handled after
            if (acc is 0 || (oldAcc != 0 && ((acc < oldAcc && rotation > 0) || (acc > oldAcc && rotation < 0))))
            {
                result++;
            }

            if (Math.Abs(rotation) >= 100)
            {
                result += Math.Abs(rotation) / 100;
            }
        }

        return $"{result}";
    }
}
