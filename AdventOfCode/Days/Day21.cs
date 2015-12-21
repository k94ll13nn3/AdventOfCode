// stylecop.header

using System;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day21 : Day
    {
        private readonly Tuple<int, int, int>[] armors;
        private readonly Tuple<int, int, int, int> boss;
        private readonly Tuple<int, int, int>[] rings;
        private readonly Tuple<int, int, int>[] weapons;

        public Day21()
        {
            this.weapons = new[]
                {
                    Tuple.Create(8, 4, 0),
                    Tuple.Create(10, 5, 0),
                    Tuple.Create(25, 6, 0),
                    Tuple.Create(40, 7, 0),
                    Tuple.Create(74, 8, 0)
                };

            this.armors = new[]
                {
                    Tuple.Create(0, 0, 0),
                    Tuple.Create(13, 0, 1),
                    Tuple.Create(31, 0, 2),
                    Tuple.Create(53, 0, 3),
                    Tuple.Create(75, 0, 4),
                    Tuple.Create(102, 0, 5)
                };

            this.rings = new[]
                {
                    Tuple.Create(0, 0, 0),
                    Tuple.Create(25, 1, 0),
                    Tuple.Create(50, 2, 0),
                    Tuple.Create(100, 3, 0),
                    Tuple.Create(20, 0, 1),
                    Tuple.Create(40, 0, 2),
                    Tuple.Create(80, 0, 3)
                };

            var lines = this.Lines.ToList();

            this.boss = Tuple.Create(
                int.Parse(lines[0].Split(' ').Last()),
                0,
                int.Parse(lines[1].Split(' ').Last()),
                int.Parse(lines[2].Split(' ').Last()));
        }

        public override object ProcessFirst()
        {
            var minCost = int.MaxValue;

            foreach (var weapon in this.weapons)
            {
                foreach (var armor in this.armors)
                {
                    for (var k = 0; k < this.rings.Length; k++)
                    {
                        for (var l = 0; l < this.rings.Length; l++)
                        {
                            if (l != 0 && l == k)
                            {
                                continue;
                            }

                            var player = Tuple.Create(
                                100,
                                weapon.Item1 + armor.Item1 + this.rings[k].Item1 + this.rings[l].Item1,
                                weapon.Item2 + armor.Item2 + this.rings[k].Item2 + this.rings[l].Item2,
                                weapon.Item3 + armor.Item3 + this.rings[k].Item3 + this.rings[l].Item3);

                            if (IsWinner(player, this.boss))
                            {
                                minCost = Math.Min(minCost, player.Item2);
                            }
                        }
                    }
                }
            }

            return minCost;
        }

        public override object ProcessSecond()
        {
            var maxCost = int.MinValue;

            foreach (var weapon in this.weapons)
            {
                foreach (var armor in this.armors)
                {
                    for (var k = 0; k < this.rings.Length; k++)
                    {
                        for (var l = 0; l < this.rings.Length; l++)
                        {
                            if (l != 0 && l == k)
                            {
                                continue;
                            }

                            var player = Tuple.Create(
                                100,
                                weapon.Item1 + armor.Item1 + this.rings[k].Item1 + this.rings[l].Item1,
                                weapon.Item2 + armor.Item2 + this.rings[k].Item2 + this.rings[l].Item2,
                                weapon.Item3 + armor.Item3 + this.rings[k].Item3 + this.rings[l].Item3);

                            if (!IsWinner(player, this.boss))
                            {
                                maxCost = Math.Max(maxCost, player.Item2);
                            }
                        }
                    }
                }
            }

            return maxCost;
        }

        private static bool IsWinner(Tuple<int, int, int, int> player, Tuple<int, int, int, int> boss)
        {
            double playerTurn = player.Item3 - boss.Item4;
            double bossTurn = boss.Item3 - player.Item4;

            if (bossTurn <= 0.0)
            {
                return true;
            }

            if (playerTurn <= 0.0)
            {
                return false;
            }

            var playerNumberOfTurns = Math.Ceiling(boss.Item1 / playerTurn);
            var bossNumberOfTurns = Math.Ceiling(player.Item1 / bossTurn);

            return playerNumberOfTurns <= bossNumberOfTurns;
        }
    }
}