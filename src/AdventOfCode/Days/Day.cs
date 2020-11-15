using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Days
{
    public abstract class Day
    {
        public static readonly Day Empty = new DummyDay();

        private readonly string _dayNumber;

        private string? _content;

        private string[]? _lines;

        protected Day() => _dayNumber = GetType().Name.Substring(3);

        public abstract string Title { get; }

        public ReadOnlySpan<char> GetContent()
        {
            return GetContentAsString().AsSpan();
        }

        public string GetContentAsString()
        {
            return _content ??= File.ReadAllText($"Inputs/Input{_dayNumber}.txt");
        }

        public int[] GetContentAsIntArray(char separator)
        {
            return Array.ConvertAll(GetContentAsString().Split(separator), int.Parse);
        }

        public long[] GetContentAsLongArray(char separator)
        {
            return Array.ConvertAll(GetContentAsString().Split(separator), long.Parse);
        }

        public IEnumerable<ReadOnlyMemory<char>> GetLines()
        {
            return GetLinesImpl();

            IEnumerable<ReadOnlyMemory<char>> GetLinesImpl()
            {
                foreach (string line in GetLinesAsStrings())
                {
                    yield return line.AsMemory();
                }
            }
        }

        public IEnumerable<string> GetLinesAsStrings()
        {
            return _lines ??= File.ReadAllLines($"Inputs/Input{_dayNumber}.txt");
        }

        public abstract string ProcessFirst();

        public abstract string ProcessSecond();

        private class DummyDay : Day
        {
            public override string Title => "<No title>";

            public override string ProcessFirst() => "<No result>";

            public override string ProcessSecond() => "<No result>";
        }
    }
}
