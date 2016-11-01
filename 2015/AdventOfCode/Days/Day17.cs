// stylecop.header
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day17 : Day
    {
        public override object ProcessFirst()
        {
            var numbers = this.Lines.Select(l => int.Parse(l)).ToList();
            return Enumerable.Range(0, 1 << numbers.Count)
                            .Select(
                              m =>
                              Enumerable.Range(0, numbers.Count)
                                        .Where(i => (m & (1 << i)) != 0)
                                        .Select(i => numbers[i]))
                            .Count(e => e.Sum() == 150);
        }

        public override object ProcessSecond()
        {
            var numbers = this.Lines.Select(l => int.Parse(l)).ToList();
            return Enumerable.Range(0, 1 << numbers.Count)
                            .Select(
                                m =>
                                Enumerable.Range(0, numbers.Count)
                                        .Where(i => (m & (1 << i)) != 0)
                                        .Select(i => numbers[i]))
                            .Where(e => e.Sum() == 150).GroupBy(i => i.Count())
                            .OrderBy(g => g.Key)
                            .First()
                            .Count();
        }
    }
}