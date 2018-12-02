using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day2 : Day
    {
        public override string Title => "Inventory Management System";

        public override string ProcessFirst()
        {
            var countTwo = 0;
            var countThree = 0;

            foreach (var line in GetLinesAsStrings())
            {
                var groups = line.GroupBy(c => c);
                countTwo += (groups.Any(g => g.Count() == 2) ? 1 : 0);
                countThree += (groups.Any(g => g.Count() == 3) ? 1 : 0);
            }

            return (countTwo * countThree).ToString();
        }

        public override string ProcessSecond()
        {
            var lines = GetLinesAsStrings().ToList();

            for (int i = 0; i < lines.Count - 1; i++)
            {
                for (int j = i + 1; j < lines.Count; j++)
                {
                    var (result, position) = Compare(lines[i], lines[j]);
                    if (result)
                    {
                        return lines[i].Substring(0, position) + lines[i].Substring(position + 1);
                    }
                }
            }

            throw new InvalidOperationException();
            
            (bool result, int position) Compare(string a, string b)
            {
                var differences = 0;
                var index = 0;
                for (int k = 0; k < a.Length; k++)
                {
                    if (a[k] != b[k])
                    {
                       differences++;
                       index = k; 
                    }

                    if (differences >= 2)
                    {
                        return (false, 0);
                    }
                }

                return (differences == 1, index);
            }
        }
    }
}