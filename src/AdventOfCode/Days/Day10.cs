using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day10 : Day
    {
        public override string Title => "Monitoring Station";

        public override string ProcessFirst()
        {
            IEnumerable<(int x, int y)> asteroids = GetAsteroidsCoordinates(GetLines().ToList());
            return Compute(asteroids).Asteroids.Count.ToString();
        }

        public override string ProcessSecond()
        {
            IEnumerable<(int x, int y)> asteroids2 = GetAsteroidsCoordinates(GetLines().ToList());
            (int stationX, int stationY, _) = Compute(asteroids2);
            var asteroids = GetAsteroidsForStation(stationX, stationY, asteroids2)
                .OrderBy(x => x.Angle)
                .ThenBy(x => x.Distance)
                .ToList();

            (int x, int y, _, _) = Sort(asteroids).Skip(199).First();

            return ((y * 100) + x).ToString();
        }

        private static IEnumerable<(int x, int y)> GetAsteroidsCoordinates(List<string> map)
        {
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j] == '#')
                    {
                        yield return (i, j);
                    }
                }
            }
        }

        private static List<Asteroid> GetAsteroidsForStation(int x, int y, IEnumerable<(int x, int y)> asteroids)
        {
            var asteroidsForStation = new List<Asteroid>();

            foreach ((int i, int j) in asteroids)
            {
                if (Math.Abs(i - x) + Math.Abs(j - y) > 0)
                {
                    // Convert coordinates to angle in rad (ant2) then in degrees, then set (-1,0) as the 0 mark (was 270) (+90) and modulus 360 to only have positives.
                    double angle = ((Math.Atan2(i - x, j - y) * (180 / Math.PI)) + 90 + 360) % 360;
                    asteroidsForStation.Add(new(i, j, angle, Math.Abs(i - x) + Math.Abs(j - y)));
                }
            }

            return asteroidsForStation;
        }

        private static IEnumerable<Asteroid> Sort(IList<Asteroid> p)
        {
            Asteroid asteroid = p[0];
            yield return asteroid;

            IList<Asteroid> rest = p.Skip(1).SkipWhile(x => x.Angle == asteroid.Angle).Concat(p.Skip(1).TakeWhile(x => x.Angle == asteroid.Angle)).ToList();
            while (rest.Count > 0)
            {
                asteroid = rest[0];
                yield return asteroid;
                rest = rest.Skip(1).SkipWhile(x => x.Angle == asteroid.Angle).Concat(p.Skip(1).TakeWhile(x => x.Angle == asteroid.Angle)).ToList();
            }
        }

        private static Station Compute(IEnumerable<(int x, int y)> asteroids)
        {
            Station station = new(-1, -1, new List<Asteroid>());

            foreach ((int i, int j) in asteroids)
            {
                var visibleAsteroids = GetAsteroidsForStation(i, j, asteroids)
                    .GroupBy(x => x.Angle)
                    .Select(x => x.OrderBy(a => a.Distance).First())
                    .ToList();

                if (visibleAsteroids.Count > station.Asteroids.Count)
                {
                    station = new(i, j, visibleAsteroids);
                }
            }

            return station;
        }

        record Asteroid(int X, int Y, double Angle, double Distance);
        record Station(int X, int Y, List<Asteroid> Asteroids);
    }
}
