using System.Linq;

namespace AdventOfCode.Days
{
    public class Day3 : Day
    {
        public override string Title => "Password Philosophy";

        public override string ProcessFirst()
        {
            string[] lines = GetLines().ToArray();
            return $"{CountTrees(lines, 1, 3)}";
        }

        public override string ProcessSecond()
        {
            string[] lines = GetLines().ToArray();
            return $"{new (int right, int down)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }.Aggregate(1L, (acc, t) => acc * CountTrees(lines, t.down, t.right))}";
        }

        private static int CountTrees(string[] lines, int rowIncrease, int columnIncrease)
        {
            int count = 0;
            for (int i = 0, j = 0; i < lines.Length; i += rowIncrease, j += columnIncrease)
            {
                if (lines[i][j % lines[0].Length] == '#')
                {
                    count++;
                }
            }

            return count;
        }
    }
}
