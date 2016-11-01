// stylecop.header
using System;
using System.Linq;

namespace AdventOfCode.Days
{
    internal class Day22 : Day
    {
        private static readonly int NumberOfSpells = 5;

        private static readonly Spell[] SpellsList =
                    {
                new Spell { Name = "Magic Missile", Cost = 53, Damage = 4 },
                new Spell { Name = "Drain", Cost = 73, Damage = 2, Heal = 2 },
                new Spell { Name = "Shield", Cost = 113, Duration = 6, Armor = 7 },
                new Spell { Name = "Poison", Cost = 173, Damage = 3, Duration = 6 },
                new Spell { Name = "Recharge", Cost = 229, Duration = 5, Mana = 101 }
            };

        private static int minCost = int.MaxValue;
        private readonly Tuple<int, int> bossBase;
        private readonly Tuple<int, int> playerBase;

        public Day22()
        {
            var lines = this.Lines.ToList();

            this.bossBase = Tuple.Create(
                int.Parse(lines[0].Split(' ').Last()),
                int.Parse(lines[1].Split(' ').Last()));

            this.playerBase = Tuple.Create(50, 500);
        }

        public override object ProcessFirst()
        {
            var activeSpells = new int[5];
            minCost = int.MaxValue;

            for (int i = 0; i < NumberOfSpells; i++)
            {
                PlayTurn(true, SpellsList[i], this.playerBase, this.bossBase, activeSpells, 0, 0);
            }

            return minCost;
        }

        public override object ProcessSecond()
        {
            var activeSpells = new int[5];
            minCost = int.MaxValue;

            for (int i = 0; i < NumberOfSpells; i++)
            {
                PlayTurn(true, SpellsList[i], this.playerBase, this.bossBase, activeSpells, 0, 1);
            }

            return minCost;
        }

        private static void PlayTurn(bool isPlayerTurn, Spell spellToCast, Tuple<int, int> player, Tuple<int, int> boss, int[] activeSpells, int combineCost, int handicap = 0)
        {
            var playerDuringTurn = Tuple.Create(player.Item1, player.Item2);
            var bossDuringTurn = Tuple.Create(boss.Item1, boss.Item2);

            if (isPlayerTurn)
            {
                playerDuringTurn = Tuple.Create(playerDuringTurn.Item1 - handicap, playerDuringTurn.Item2);

                // player is dead
                if (playerDuringTurn.Item1 <= 0)
                {
                    return;
                }
            }

            var damages = 0;
            var heals = 0;
            var manaRecovered = 0;

            for (int i = 0; i < NumberOfSpells; i++)
            {
                var s = SpellsList[i];
                if (activeSpells[s.Id] > 0)
                {
                    damages += s.Damage;
                    heals += s.Heal;
                    manaRecovered += s.Mana;
                }
            }

            playerDuringTurn = Tuple.Create(playerDuringTurn.Item1 + heals, playerDuringTurn.Item2 + manaRecovered);
            bossDuringTurn = Tuple.Create(bossDuringTurn.Item1 - damages, bossDuringTurn.Item2);

            var activeSpellsDuringTurn = new int[5];
            for (int i = 0; i < NumberOfSpells; i++)
            {
                var spell = SpellsList[i];
                activeSpellsDuringTurn[i] = Math.Max(0, activeSpells[spell.Id] - 1);
            }

            // boss is dead
            if (bossDuringTurn.Item1 <= 0)
            {
                minCost = Math.Min(combineCost, minCost);
                return;
            }

            if (!isPlayerTurn)
            {
                playerDuringTurn = Tuple.Create(playerDuringTurn.Item1 - bossDuringTurn.Item2, playerDuringTurn.Item2);

                var armor = 0;
                for (int i = 0; i < NumberOfSpells; i++)
                {
                    var s = SpellsList[i];
                    if (activeSpells[s.Id] > 0)
                    {
                        armor += s.Armor;
                    }
                }

                playerDuringTurn = Tuple.Create(playerDuringTurn.Item1 + armor, playerDuringTurn.Item2);

                // player is dead
                if (playerDuringTurn.Item1 <= 0)
                {
                    return;
                }

                PlayTurn(true, spellToCast, playerDuringTurn, bossDuringTurn, activeSpellsDuringTurn, combineCost, handicap);
            }
            else
            {
                combineCost += spellToCast.Cost;

                // Cannot pay spell
                if (playerDuringTurn.Item2 < spellToCast.Cost || combineCost >= minCost)
                {
                    return;
                }

                playerDuringTurn = Tuple.Create(playerDuringTurn.Item1, playerDuringTurn.Item2 - spellToCast.Cost);
                activeSpellsDuringTurn[spellToCast.Id] = spellToCast.Duration;

                if (spellToCast.Duration == 0)
                {
                    playerDuringTurn = Tuple.Create(playerDuringTurn.Item1 + spellToCast.Heal, playerDuringTurn.Item2);
                    bossDuringTurn = Tuple.Create(bossDuringTurn.Item1 - spellToCast.Damage, bossDuringTurn.Item2);
                }

                // boss is dead
                if (bossDuringTurn.Item1 <= 0)
                {
                    minCost = Math.Min(combineCost, minCost);
                    return;
                }

                for (int i = 0; i < NumberOfSpells; i++)
                {
                    var spell = SpellsList[i];
                    PlayTurn(false, spell, playerDuringTurn, bossDuringTurn, activeSpellsDuringTurn, combineCost, handicap);
                }
            }
        }
    }

    internal class Spell
    {
        private static int nextId;

        public Spell()
        {
            this.Id = nextId;
            nextId++;
        }

        public int Armor { get; set; }

        public int Cost { get; set; }

        public int Damage { get; set; }

        public int Duration { get; set; }

        public int Heal { get; set; }

        public int Id { get; set; }

        public int Mana { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}