// stylecop.header

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    internal class Day6 : Day
    {
        public override object ProcessFirst()
        {
            var lights = new bool[1000, 1000];

            foreach (var line in this.Lines)
            {
                ProcessCommand(line, lights);
            }

            return lights.Cast<bool>().ToList().Count(c => c);
        }

        public override object ProcessSecond()
        {
            var lights = new int[1000, 1000];

            foreach (var line in this.Lines)
            {
                ProcessCommand(line, lights);
            }

            return lights.Cast<int>().ToList().Sum();
        }

        private static void ChangeLights(bool[,] lights, Tuple<int, int> start, Tuple<int, int> end, Func<bool, bool> action)
        {
            for (int i = start.Item1; i <= end.Item1; i++)
            {
                for (int j = start.Item2; j <= end.Item2; j++)
                {
                    lights[i, j] = action(lights[i, j]);
                }
            }
        }

        private static void ChangeLights(int[,] lights, Tuple<int, int> start, Tuple<int, int> end, Func<int, int> action)
        {
            for (int i = start.Item1; i <= end.Item1; i++)
            {
                for (int j = start.Item2; j <= end.Item2; j++)
                {
                    lights[i, j] = action(lights[i, j]);
                }
            }
        }

        private static Tuple<int, int> GetRange(string range)
        {
            var i = range.Split(',');

            var tuple = Tuple.Create(int.Parse(i[0]), int.Parse(i[1]));

            return tuple;
        }

        private static void ProcessCommand<T>(string s, T lights)
        {
            var regex = new Regex(@"^(turn on|toggle|turn off) (\d+,\d+) through (\d+,\d+)$");
            var match = regex.Match(s);
            if (match.Success)
            {
                switch (match.Groups[1].Value)
                {
                    case "turn on":
                        if (lights is bool[,])
                        {
                            ChangeLights(lights as bool[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => true);
                        }
                        else if (lights is int[,])
                        {
                            ChangeLights(lights as int[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => c + 1);
                        }

                        break;

                    case "toggle":
                        if (lights is bool[,])
                        {
                            ChangeLights(lights as bool[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => !c);
                        }
                        else if (lights is int[,])
                        {
                            ChangeLights(lights as int[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => c + 2);
                        }

                        break;

                    case "turn off":
                        if (lights is bool[,])
                        {
                            ChangeLights(lights as bool[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => false);
                        }
                        else if (lights is int[,])
                        {
                            ChangeLights(lights as int[,], GetRange(match.Groups[2].Value), GetRange(match.Groups[3].Value), (c) => c > 0 ? c - 1 : c);
                        }

                        break;
                }
            }
        }
    }
}