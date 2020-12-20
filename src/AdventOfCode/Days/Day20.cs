using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day20 : Day
    {
        public override string Title => "Jurassic Jigsaw";

        public override string ProcessFirst()
        {
            var input = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new Tile(
                    int.Parse(x[0].Split(new[] { ':', ' ' })[1]),
                    x[1],
                    x[^1],
                    new string(x[1..].Select(s => s[^1]).ToArray()),
                    new string(x[1..].Select(s => s[0]).ToArray())
                    ))
                .ToList();

            return $"{input.Where(x => CountSides(input, x) == 2).Aggregate(1L, (acc, v) => acc * v.Id)}";
        }

        public override string ProcessSecond()
        {
            return "TODO";
        }

        private static int CountSides(List<Tile> input, Tile tileToCheck)
        {
            int count = 0;
            foreach (Tile tile in input.Where(x => x.Id != tileToCheck.Id))
            {
                foreach (string side in tile.GetAllSides())
                {
                    if (tileToCheck.Right == side)
                    {
                        count++;
                    }

                    if (tileToCheck.Bottom == side)
                    {
                        count++;
                    }

                    if (tileToCheck.Top == side)
                    {
                        count++;
                    }

                    if (tileToCheck.Left == side)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        record Tile(int Id, string Top, string Right, string Bottom, string Left)
        {
            public List<string> GetAllSides()
            {
                return new List<string>
                {
                    Top,
                    Right,
                    Bottom,
                    Left,
                    new string(Top.Reverse().ToArray()),
                    new string(Right.Reverse().ToArray()),
                    new string(Bottom.Reverse().ToArray()),
                    new string(Left.Reverse().ToArray()),
                };
            }
        }
    }
}
