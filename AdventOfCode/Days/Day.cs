// stylecop.header
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    internal abstract class Day
    {
        protected Day()
        {
            var i = int.Parse(this.GetType().Name.Substring(3));
            this.Content = File.ReadAllText($"Inputs/Input{i}.txt");
            this.Lines = File.ReadAllLines($"Inputs/Input{i}.txt");
        }

        public string Content { get; }

        public IEnumerable<string> Lines { get; }

        public abstract object ProcessFirst();

        public abstract object ProcessSecond();
    }
}