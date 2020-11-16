using System.Linq;

namespace AdventOfCode.Days
{
    public class Day1 : Day
    {
        public override string Title => "The Tyranny of the Rocket Equation";

        public override string ProcessFirst()
        {
            return GetLines().Select(int.Parse).Select(x => (x / 3) - 2).Sum().ToString();
        }

        public override string ProcessSecond()
        {
            return GetLines().Select(int.Parse).Select(x => GetFuelRec(x, 0)).Sum().ToString();

            static int GetFuelRec(int mass, int acc)
            {
                int fuel = (mass / 3) - 2;
                return fuel switch
                {
                    <= 0 => acc,
                    _ => GetFuelRec(fuel, fuel + acc)
                };
            }
        }
    }
}
