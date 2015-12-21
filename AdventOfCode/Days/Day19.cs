// stylecop.header

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day19 : Day
    {
        private static int totalStep;
        private readonly IEnumerable<Tuple<string, string>> transformations;

        public Day19()
        {
            var tmpTransformations = new List<Tuple<string, string>>();
            foreach (var line in this.Lines.Take(this.Lines.Count() - 2))
            {
                var tokens = line.Split(' ');
                tmpTransformations.Add(Tuple.Create(tokens[0], tokens[2]));
            }

            this.transformations = tmpTransformations;
        }

        public override object ProcessFirst()
        {
            var molecule = this.Lines.Last();

            var createdMolecules = new List<string>();

            foreach (var transformation in this.transformations)
            {
                createdMolecules.AddRange(ComputeReplacements(molecule, transformation));
            }

            return createdMolecules.Distinct().Count();
        }

        public override object ProcessSecond()
        {
            var wantedMolecule = this.Lines.Last();

            var createdMolecules = new List<string>();

            return FindMatchRec("e", wantedMolecule, this.transformations, 1);
        }

        private static IEnumerable<string> ComputeReplacements(string molecule, Tuple<string, string> replacement)
        {
            var createdMolecules = new List<string>();
            var currentIndex = 0;

            currentIndex = molecule.IndexOf(replacement.Item1, StringComparison.Ordinal);

            while (currentIndex != -1)
            {
                var tmpMolecule = molecule.Remove(currentIndex, replacement.Item1.Length);
                createdMolecules.Add(tmpMolecule.Insert(currentIndex, replacement.Item2));
                currentIndex = molecule.IndexOf(replacement.Item1, currentIndex + 1, StringComparison.Ordinal);
            }

            return createdMolecules;
        }

        private static int FindMatchRec(string molecule, string wantedMolecule, IEnumerable<Tuple<string, string>> replacements, int step)
        {
            // Only 100000 molecules are computed. Pray \o/.
            totalStep++;
            if (totalStep >= 100000)
            {
                return int.MaxValue;
            }

            var createdMolecules = new List<string>();
            var steps = new List<int>();

            foreach (var replacement in replacements.Select(t => Tuple.Create(t.Item2, t.Item1)))
            {
                createdMolecules.AddRange(ComputeReplacements(wantedMolecule, replacement));
            }

            if (createdMolecules.Any(mol => mol == molecule))
            {
                return step;
            }

            if (!createdMolecules.Any())
            {
                return int.MaxValue;
            }

            foreach (var mol in createdMolecules.Distinct())
            {
                steps.Add(FindMatchRec(molecule, mol, replacements, step + 1));
            }

            return steps.Min();
        }
    }
}