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
            var stack = new Stack<char>();
            foreach (var c in polymer)
            {
                if (stack.Count == 0)
                {
                    stack.Push(c);
                }
                else
                {
                    var peek = stack.Peek();
                    if (((c + 32) == peek) || ((peek + 32) == c))
                    {
                        stack.Pop();
                    }
                    else
                    {
                        stack.Push(c);
                    }
                }
            }

            return stack.Count;
        }
    }
}