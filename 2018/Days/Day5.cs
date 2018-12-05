using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day5 : Day
    {
        public override string Title => "Alchemical Reduction";

        public override string ProcessFirst()
        {
            return Collapse(GetContentAsString()).ToString();
        }

        public override string ProcessSecond()
        {
            var minimum = int.MaxValue;
            var content = GetContentAsString();
            for (int i = 'a'; i <= 'z'; i++)
            {
                var replacedContent = content.Replace($"{(char)i}", "").Replace($"{(char)(i - 32)}", "");
                minimum = Math.Min(minimum, Collapse(replacedContent));
            }

            return minimum.ToString();
        }

        private int Collapse(string polymer)
        {
            var content = polymer.ToList();
            bool hasModifications = true;

            while (hasModifications)
            {
                hasModifications = false;

                for (int i = content.Count - 1; i >= 1; i--)
                {
                    if ((content[i] + 32) == content[i - 1] || (content[i - 1] + 32) == content[i])
                    {
                        content.RemoveAt(i);
                        content.RemoveAt(i - 1);
                        i--;
                        hasModifications = true;
                    }
                }
            }

            return content.Count;
        }
    }
}