using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day12 : Day
    {
        public override string Title => "The N-Body Problem";

        public override string ProcessFirst()
        {
            IList<int[]> moons = GetLinesAsStrings()
                .Select(x => x.Split(new[] { ',', '=', '<', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => GetMoon(int.Parse(x[1]), int.Parse(x[3]), int.Parse(x[5]), 0, 0, 0))
                .ToList();

            int[] moon0 = moons[0];
            int[] moon1 = moons[1];
            int[] moon2 = moons[2];
            int[] moon3 = moons[3];

            for (long i = 0; i < 1000; i++)
            {
                UpdateVelocity(moon0, moon1, moon2, moon3);
                UpdateVelocity(moon1, moon0, moon2, moon3);
                UpdateVelocity(moon2, moon0, moon1, moon3);
                UpdateVelocity(moon3, moon0, moon1, moon2);

                ApplyVelocity(moon0);
                ApplyVelocity(moon1);
                ApplyVelocity(moon2);
                ApplyVelocity(moon3);
            }

            return (GetEnergy(moon0) + GetEnergy(moon1) + GetEnergy(moon2) + GetEnergy(moon3)).ToString();
        }

        public override string ProcessSecond()
        {
            IList<int[]> moons = GetLinesAsStrings()
                .Select(x => x.Split(new[] { ',', '=', '<', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => GetMoon(int.Parse(x[1]), int.Parse(x[3]), int.Parse(x[5]), 0, 0, 0))
                .ToList();

            int[] moon0 = moons[0];
            int[] moon1 = moons[1];
            int[] moon2 = moons[2];
            int[] moon3 = moons[3];

            HashSet<(int, int, int, int, int, int, int, int)> hashSetX = new();
            HashSet<(int, int, int, int, int, int, int, int)> hashSetY = new();
            HashSet<(int, int, int, int, int, int, int, int)> hashSetZ = new();
            int cycleX = 0;
            int cycleY = 0;
            int cycleZ = 0;

            while (cycleX == 0 || cycleY == 0 || cycleZ == 0)
            {
                (int, int, int, int, int, int, int, int) t = (moon0[0], moon0[3], moon1[0], moon1[3], moon2[0], moon2[3], moon3[0], moon3[3]);
                if (!hashSetX.Contains(t))
                {
                    hashSetX.Add(t);
                }
                else
                {
                    cycleX = hashSetX.Count;
                }

                t = (moon0[1], moon0[4], moon1[1], moon1[4], moon2[1], moon2[4], moon3[1], moon3[4]);
                if (!hashSetY.Contains(t))
                {
                    hashSetY.Add(t);
                }
                else
                {
                    cycleY = hashSetY.Count;
                }

                t = (moon0[2], moon0[5], moon1[2], moon1[5], moon2[2], moon2[5], moon3[2], moon3[5]);
                if (!hashSetZ.Contains(t))
                {
                    hashSetZ.Add(t);
                }
                else
                {
                    cycleZ = hashSetZ.Count;
                }

                UpdateVelocity(moon0, moon1, moon2, moon3);
                UpdateVelocity(moon1, moon0, moon2, moon3);
                UpdateVelocity(moon2, moon0, moon1, moon3);
                UpdateVelocity(moon3, moon0, moon1, moon2);

                ApplyVelocity(moon0);
                ApplyVelocity(moon1);
                ApplyVelocity(moon2);
                ApplyVelocity(moon3);
            }

            return Lcm(cycleX, Lcm(cycleY, cycleZ)).ToString();
        }

        private static long Lcm(long m, long n)
        {
            return Math.Abs(m * n) / Gcd(m, n);
        }

        private static long Gcd(long m, long n)
        {
            return n == 0 ? Math.Abs(m) : Gcd(n, m % n);
        }

        private static void ApplyVelocity(int[] data)
        {
            data[0] += data[3];
            data[1] += data[4];
            data[2] += data[5];
        }

        private static void UpdateVelocity(int[] data, int[] moon1Data, int[] moon2Data, int[] moon3Data)
        {
            data[3] = data[3] + moon1Data[0].CompareTo(data[0]) + moon2Data[0].CompareTo(data[0]) + moon3Data[0].CompareTo(data[0]);
            data[4] = data[4] + moon1Data[1].CompareTo(data[1]) + moon2Data[1].CompareTo(data[1]) + moon3Data[1].CompareTo(data[1]);
            data[5] = data[5] + moon1Data[2].CompareTo(data[2]) + moon2Data[2].CompareTo(data[2]) + moon3Data[2].CompareTo(data[2]);
        }

        private static int GetEnergy(int[] data)
        {
            return (Math.Abs(data[0]) + Math.Abs(data[1]) + Math.Abs(data[2])) * (Math.Abs(data[3]) + Math.Abs(data[4]) + Math.Abs(data[5]));
        }

        private static int[] GetMoon(int x, int y, int z, int vX, int vY, int vZ)
        {
            int[] data = new int[6];
            data[0] = x;
            data[1] = y;
            data[2] = z;
            data[3] = vX;
            data[4] = vY;
            data[5] = vZ;
            return data;
        }
    }
}
