using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    internal abstract class Day
    {
        protected Day(int i)
        {
            this.Content = File.ReadAllText($"Inputs/Input{i}.txt");
            this.Lines = File.ReadAllLines($"Inputs/Input{i}.txt");
        }

        public string Content { get; }

        public IEnumerable<string> Lines { get; set; }

        public abstract object ProcessFirst();

        public abstract object ProcessSecond();
    }
}