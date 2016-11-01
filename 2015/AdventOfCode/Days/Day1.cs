// stylecop.header

using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day1 : Day
    {
        public override object ProcessFirst()
        {
            var count = this.Content.Count(c => c == '(') - this.Content.Count(c => c == ')');

            return count;
        }

        public override object ProcessSecond()
        {
            var count = 0;

            for (int i = 0; i < this.Content.Length; i++)
            {
                count += this.Content[i] == '(' ? 1 : -1;
                if (count == -1)
                {
                    return i + 1;
                }
            }

            return 42;
        }
    }
}