using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day14 : Day
    {
        public override string Title => "Space Stoichiometry";

        [SuppressMessage("Major Code Smell", "S2589", Justification = "False positive.")]
        public override string ProcessFirst()
        {
            Dictionary<string, Reaction> reactions = ParseReactions();
            Reaction fuelReaction = reactions["FUEL"];

            Dictionary<string, int> inputsNeeded = reactions
                .SelectMany(x => x.Value.Inputs)
                .Select(x => x.Name)
                .Distinct()
                .Where(x => x != "ORE")
                .ToDictionary(x => x, _ => 0);

            foreach (Material materiel in fuelReaction.Inputs)
            {
                inputsNeeded[materiel.Name] += materiel.Quantity;
            }

            bool inputsHaveChanged = true;
            while (inputsHaveChanged)
            {
                inputsHaveChanged = false;
                foreach ((string name, int quantity) in inputsNeeded.Where(x => x.Value > 0))
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

            int oreNeeded = 0;
            foreach ((string name, int quantity) in inputsNeeded.Where(x => x.Value > 0))
            {
                Reaction r = reactions[name];
                int x = (int)Math.Ceiling(quantity / (double)r.Output.Quantity);
                oreNeeded += r.Inputs[0].Quantity * x;
            }

            return oreNeeded.ToString();
        }

        [SuppressMessage("Major Code Smell", "S2589", Justification = "False positive.")]
        public override string ProcessSecond()
        {
            Dictionary<string, Reaction> reactions = ParseReactions();
            Reaction fuelReaction = reactions["FUEL"];

            Dictionary<string, int> inputsNeeded = reactions
                .SelectMany(x => x.Value.Inputs)
                .Select(x => x.Name)
                .Distinct()
                .Where(x => x != "ORE")
                .ToDictionary(x => x, _ => 0);

            long fuel = 0;
            long oreConsumed = 0;
            while (oreConsumed < 1000000000000)
            {
                foreach (Material materiel in fuelReaction.Inputs)
                {
                    inputsNeeded[materiel.Name] += materiel.Quantity;
                }

                bool inputsHaveChanged = true;
                while (inputsHaveChanged)
                {
                    inputsHaveChanged = false;
                    foreach ((string name, int quantity) in inputsNeeded.Where(x => x.Value > 0))
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

                foreach ((string name, int quantity) in inputsNeeded.Where(x => x.Value > 0))
                {
                    Reaction r = reactions[name];
                    int x = (int)Math.Ceiling(quantity / (double)r.Output.Quantity);
                    oreConsumed += r.Inputs[0].Quantity * x;
                    inputsNeeded[name] -= x * r.Output.Quantity;
                }

                fuel++;

                if (inputsNeeded.All(x => x.Value == 0))
                {
                    Console.WriteLine("ok");
                    long x = 1000000000000 / oreConsumed;
                    oreConsumed *= x;
                    fuel *= x;
                }
            }

            return (fuel - 1).ToString(); // Minus 1 because of the last increase when ore is too much.
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
                return new(int.Parse(s[0]), s[1]);
            }
        }

        private Dictionary<string, Reaction> ParseReactions()
        {
            IEnumerable<string> lines = GetLines();

            var reactions = new List<Reaction>();
            foreach (string line in lines)
            {
                reactions.Add(ParseReaction(line));
            }

            return reactions.ToDictionary(x => x.Output.Name, x => x);
        }

        record Material(int Quantity, string Name);
        record Reaction(List<Material> Inputs, Material Output);
    }
}
