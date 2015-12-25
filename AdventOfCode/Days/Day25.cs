using System;

namespace AdventOfCode.Days
{
    internal class Day25 : Day
    {
        public Day25()
            : base("To continue, please consult the code grid in the manual.  Enter the code at row 2978, column 3083.")
        {
        }

        public override object ProcessFirst()
        {
            var tokens = this.Content.Split(new char[] { '.', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var row = int.Parse(tokens[15]);
            var column = int.Parse(tokens[17]);

            // r 3 c 4 :
            // (1 + 2 + 3 + 4) + ((1 + 2 + 3 + 4 + 5) - ( 1 + 2 + 3))
            // 1 + 2 + 3 + 4 + 5 + n
            var number = column + ((row + column - 2) * (row + column - 1) / 2);

            ulong code = 20151125;

            while (number > 1)
            {
                number--;
                code = (code * 252533) % 33554393;
            }

            return code;
        }

        public override object ProcessSecond()
        {
            return "Merry Christmas !";
        }
    }
}