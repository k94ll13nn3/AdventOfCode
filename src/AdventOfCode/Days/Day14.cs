using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day14 : Day
    {
        public override string Title => "Space Stoichiometry";

        public override string ProcessFirst()
        {
            return GetCostOfFuel(1).ToString();
        }

        public override string ProcessSecond()
        {
            long possibleCost = 1000000000000 / GetCostOfFuel(1);
            long max = 0;
            for (long i = possibleCost; i < possibleCost * 2; i += 10000)
            {
                if (GetCostOfFuel(i) > 1000000000000)
                {
                    break;
                }

                max = i;
            }

            foreach (int item in new[] { 1000, 100, 100, 10 })
            {
                for (long i = max - item; i < max + item; i += item / 10)
                {
                    if (GetCostOfFuel(i) > 1000000000000)
                    {
                        break;
                    }

                    max = i;
                }
            }

            return max.ToString();
        }

        private static Reaction ParseReaction(string line)
        {
            string[] parts = line.Split("=>", StringSplitOptions.RemoveEmptyEntries);
            var materials = new List<Material>();
            foreach (string inputMaterial in parts[0].Split(','))
            {
                materials.Add(ParseMaterial(inputMaterial));
            }

            return new(materials, ParseMaterial(parts[1]));

            static Material ParseMaterial(string materialLine)
            {
                string[] s = materialLine.Trim().Split(' ');
                return new(long.Parse(s[0]), s[1]);
            }
        }

        [SuppressMessage("Major Code Smell", "S2589", Justification = "False positive.")]
        private long GetCostOfFuel(long fuelNeeded)
        {
            Dictionary<string, Reaction> reactions = ParseReactions(fuelNeeded);
            Reaction fuelReaction = reactions["FUEL"];

            Dictionary<string, long> inputsNeeded = reactions
                .SelectMany(x => x.Value.Inputs)
                .Select(x => x.Name)
                .Distinct()
                .Where(x => x != "ORE")
                .ToDictionary(x => x, _ => 0L);

            foreach (Material materiel in fuelReaction.Inputs)
            {
                inputsNeeded[materiel.Name] += materiel.Quantity * fuelReaction.Output.Quantity;
            }

            bool inputsHaveChanged = true;
            while (inputsHaveChanged)
            {
                inputsHaveChanged = false;
                foreach ((string name, long quantity) in inputsNeeded.Where(x => x.Value > 0))
                {
                    Reaction r = reactions[name];
                    if (r.Inputs[0].Name != "ORE")
                    {
                        inputsHaveChanged = true;
                        int x = (int)Math.Ceiling(quantity / (double)r.Output.Quantity);
                        inputsNeeded[name] -= x * r.Output.Quantity; // might produce more than needed
                        foreach (Material item in r.Inputs)
                        {
                            inputsNeeded[item.Name] += x * item.Quantity;
                        }
                    }
                }
            }

            long oreNeeded = 0;
            foreach ((string name, long quantity) in inputsNeeded.Where(x => x.Value > 0))
            {
                Reaction r = reactions[name];
                long x = (long)Math.Ceiling(quantity / (double)r.Output.Quantity);
                oreNeeded += r.Inputs[0].Quantity * x;
            }

            return oreNeeded;
        }

        private Dictionary<string, Reaction> ParseReactions(long fuelNeeded)
        {
            IEnumerable<string> lines = GetLines();

            var reactions = new List<Reaction>();
            foreach (string line in lines)
            {
                reactions.Add(ParseReaction(line.Replace("1 FUEL", $"{fuelNeeded} FUEL")));
            }

            return reactions.ToDictionary(x => x.Output.Name, x => x);
        }

        record Material(long Quantity, string Name);
        record Reaction(List<Material> Inputs, Material Output);
    }
}
