using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    internal abstract class Day
    {
        protected Day(int i)
        {
            Content = File.ReadAllText($"Inputs/Input{i}.txt");
            Lines = File.ReadAllLines($"Inputs/Input{i}.txt");
        }

        public string Content { get; }

        public IEnumerable<string> Lines { get; }

        public abstract object ProcessFirst();

        public abstract object ProcessSecond();
    }
}