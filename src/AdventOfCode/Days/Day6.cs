using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day6 : Day
    {
        public override string Title => "Universal Orbit Map";

        record Planet(string Name, int Depth, List<Planet> Orbits)
        {
            public IEnumerable<Planet> GetOrbits()
            {
                return GetOrbitsImpl().SelectMany(x => x);

                IEnumerable<IEnumerable<Planet>> GetOrbitsImpl()
                {
                    foreach (Planet item in Orbits)
                    {
                        yield return item.GetOrbits();
                    }

                    yield return Orbits;
                }
            }
        }

        public override string ProcessFirst()
        {
            Planet com = GetSystem();

            return (com.GetOrbits().Select(x => x.GetOrbits().Count()).Sum() + com.GetOrbits().Count()).ToString();
        }

        public override string ProcessSecond()
        {
            Planet com = GetSystem();
            Planet chosen = com;
            Planet you = com.GetOrbits().First(x => x.Name == "YOU");
            Planet san = com.GetOrbits().First(x => x.Name == "SAN");

            foreach (Planet planet in com.GetOrbits())
            {
                IEnumerable<Planet> orbitsOfPlanet = planet.GetOrbits();
                if (planet.Depth > chosen.Depth && orbitsOfPlanet.Contains(you) && orbitsOfPlanet.Contains(san))
                {
                    chosen = planet;
                }
            }

            return (you.Depth + san.Depth - (2 * chosen.Depth) - 2).ToString();
        }

        [SuppressMessage("Major Code Smell", "S1854", Justification = "False positive, see https://github.com/SonarSource/sonar-dotnet/issues/3126")]
        private Planet GetSystem()
        {
            ILookup<string, string> lines = GetLinesAsStrings().ToLookup(x => x.Split(')')[0], x => x.Split(')')[1]);

            return new Planet("COM", 0, GetPlanets("COM", 1));

            List<Planet> GetPlanets(string name, int depth)
            {
                List<Planet> planets = new();
                foreach (string planetInOrbit in lines[name])
                {
                    planets.Add(new Planet(planetInOrbit, depth, GetPlanets(planetInOrbit, depth + 1)));
                }

                return planets;
            }
        }
    }
}
