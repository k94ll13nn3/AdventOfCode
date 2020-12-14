using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day14 : Day
    {
        public override string Title => "Docking Data";

        public override string ProcessFirst()
        {
            IEnumerable<string> lines = GetLines();

            var mem = new Dictionary<int, string>();
            string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            foreach (string line in lines)
            {
                (string cmd, string content) = line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (cmd == "mask")
                {
                    mask = content;
                }
                else
                {
                    (_, string index) = cmd.Split(new[] { '[', ']' });
                    mem[int.Parse(index)] = ApplyMask(mask, int.Parse(content));
                }
            }

            return $"{mem.Select(x => Convert.ToInt64(x.Value, 2)).Sum()}";

            static string ApplyMask(string mask, int content)
            {
                char[] result = new char[mask.Length];
                string contentAsBinary = Convert.ToString(content, 2).PadLeft(mask.Length, '0');
                for (int i = 0; i < mask.Length; i++)
                {
                    result[i] = mask[i] != 'X' ? mask[i] : contentAsBinary[i];
                }

                return new string(result);
            }
        }

        public override string ProcessSecond()
        {
            IEnumerable<string> lines = GetLines();

            var mem = new Dictionary<long, long>();
            string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            foreach (string line in lines)
            {
                (string cmd, string content) = line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (cmd == "mask")
                {
                    mask = content;
                }
                else
                {
                    (_, string index) = cmd.Split(new[] { '[', ']' });
                    foreach (long i in GetAllIndexes(mask, int.Parse(index)))
                    {
                        mem[i] = long.Parse(content);
                    }
                }
            }

            return $"{mem.Values.Sum()}";
        }

        private static List<long> GetAllIndexes(string mask, int index)
        {
            return GetAllMasks(ApplyMask(mask, index), 0).Select(x => Convert.ToInt64(x, 2)).ToList();

            static List<string> GetAllMasks(string currentMask, int ind)
            {
                var masks = new List<string>();

                int i;

                for (i = ind; i < currentMask.Length; i++)
                {
                    if (currentMask[i] == 'X')
                    {
                        masks.AddRange(GetAllMasks(currentMask[0..i] + "0" + currentMask[(i + 1)..], i + 1));
                        masks.AddRange(GetAllMasks(currentMask[0..i] + "1" + currentMask[(i + 1)..], i + 1));
                        break;
                    }
                }

                if (i == currentMask.Length)
                {
                    masks.Add(currentMask);
                }

                return masks;
            }

            static string ApplyMask(string mask, int content)
            {
                char[] result = new char[mask.Length];
                string contentAsBinary = Convert.ToString(content, 2).PadLeft(mask.Length, '0');
                for (int i = 0; i < mask.Length; i++)
                {
                    result[i] = mask[i] != '0' ? mask[i] : contentAsBinary[i];
                }

                return new string(result);
            }
        }
    }
}
