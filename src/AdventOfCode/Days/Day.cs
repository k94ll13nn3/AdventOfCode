using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    public abstract class Day
    {
        private readonly string _dayNumber;

        private string? _content;

        private string[]? _lines;

        protected Day() => _dayNumber = GetType().Name.Substring(3);

        public abstract string Title { get; }

        public string GetContent()
        {
            return _content ??= File.ReadAllText($"Inputs/Input{_dayNumber}.txt");
        }

        public IEnumerable<string> GetLines()
        {
            return _lines ??= File.ReadAllLines($"Inputs/Input{_dayNumber}.txt");
        }

        public abstract string ProcessFirst();

        public abstract string ProcessSecond();
    }
}
