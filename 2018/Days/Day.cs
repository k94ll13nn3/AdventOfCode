using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    public abstract class Day
    {
        public static readonly Day Empty = new DummyDay();

        private readonly string _dayNumber;

        private string _content;

        private string[] _lines;

        protected Day() => _dayNumber = GetType().Name.Substring(3);

        public ReadOnlySpan<char> GetContent()
        {
            if (_content is null)
            {
                _content = File.ReadAllText($"Inputs/Input{_dayNumber}.txt");
            }

            return _content.AsSpan();
        }

        public IEnumerable<ReadOnlyMemory<char>> GetLines()
        {
            if (_lines is null)
            {
                _lines = File.ReadAllLines($"Inputs/Input{_dayNumber}.txt");
            }

            return GetLinesImpl();

            IEnumerable<ReadOnlyMemory<char>> GetLinesImpl()
            {
                foreach (string line in _lines)
                {
                    yield return line.AsMemory();
                }
            }
        }

        public abstract string ProcessFirst();

        public abstract string ProcessSecond();

        private class DummyDay : Day
        {
            public override string ProcessFirst() => "Dummy";

            public override string ProcessSecond() => "Dummy";
        }
    }
}