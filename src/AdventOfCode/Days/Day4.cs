using System.Linq;

namespace AdventOfCode.Days
{
    public class Day4 : Day
    {
        public override string Title => "Secure Container";

        public override string ProcessFirst()
        {
            const int lower = 272091;
            const int upper = 815432;
            int count = 0;

            for (int i = lower + 1; i < upper; i++)
            {
                string password = i.ToString();
                bool neverDecrease = true;
                for (int j = 1; j < password.Length; j++)
                {
                    if (password[j] < password[j - 1])
                    {
                        neverDecrease = false;
                        break;
                    }
                }

                if (neverDecrease && password.GroupBy(x => x).Any(x => x.Count() >= 2))
                {
                    count++;
                }
            }

            return count.ToString();
        }

        public override string ProcessSecond()
        {
            const int lower = 272091;
            const int upper = 815432;
            int count = 0;

            for (int i = lower + 1; i < upper; i++)
            {
                string password = i.ToString();
                bool neverDecrease = true;
                for (int j = 1; j < password.Length; j++)
                {
                    if (password[j] < password[j - 1])
                    {
                        neverDecrease = false;
                        break;
                    }
                }

                if (neverDecrease && password.GroupBy(x => x).Any(x => x.Count() == 2))
                {
                    count++;
                }
            }

            return count.ToString();
        }
    }
}
