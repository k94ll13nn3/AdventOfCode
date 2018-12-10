using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day10 : Day
    {
        private readonly Regex pattern = new Regex(@"position=<([\d\s-]+),([\d\s-]+)> velocity=<([\d\s-]+),([\d\s-]+)>", RegexOptions.Compiled);

        public override string Title => "The Stars Align";

        public override string ProcessFirst()
        {
            var points = GetLinesAsStrings().Select(Parse);

            var time = 0;
            var sizeX = int.MaxValue - 1;
            var sizeY = int.MaxValue - 1;
            var currentSizeX = int.MaxValue;
            var currentSizeY = int.MaxValue;
            var sizeXMin = 0;
            var sizeXMax = 0;
            var sizeYMin = 0;
            var sizeYMax = 0;
            while (currentSizeX > sizeX && currentSizeY > sizeY && sizeX > 65)
            {
                currentSizeX = sizeX;
                currentSizeY = sizeY;
                time++;
                sizeXMin = points.Min(p => p.X + time * p.VelocityX);
                sizeXMax = points.Max(p => p.X + time * p.VelocityX);
                sizeYMin = points.Min(p => p.Y + time * p.VelocityY);
                sizeYMax = points.Max(p => p.Y + time * p.VelocityY);
                sizeX = (sizeXMax - sizeXMin);
                sizeY = (sizeYMax - sizeYMin);
            }


            var map = new char[sizeY + 1][];
            for (int i = 0; i < sizeY + 1; i++)
            {
                map[i] = new char[sizeX + 1];
            }

            foreach (var point in points)
            {
                map[point.Y + time * point.VelocityY - sizeYMin][point.X + time * point.VelocityX - sizeXMin] = '#';
            }

            for (int i = 0; i < sizeY + 1; i++)
            {
                for (int j = 0; j < sizeX + 1; j++)
                {
                    Console.Write(map[i][j] == '#' ? '#' : '.');
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine(time);

            time++;

            return "";
        }

        public override string ProcessSecond()
        {
            return "";
        }

        private Point Parse(string line)
        {
            var match = pattern.Match(line);
            if (match.Success)
            {
                return new Point
                {
                    X = int.Parse(match.Groups[1].Value),
                    Y = int.Parse(match.Groups[2].Value),
                    VelocityX = int.Parse(match.Groups[3].Value),
                    VelocityY = int.Parse(match.Groups[4].Value),
                };
            }

            return null;
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
    }
}