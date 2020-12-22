using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day22 : Day
    {
        public override string Title => "Crab Combat";

        public override string ProcessFirst()
        {
            var players = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse))
                .Select(x => new Queue<int>(x))
                .ToList();

            while (players[0].Count > 0 && players[1].Count > 0)
            {
                int p1 = players[0].Dequeue();
                int p2 = players[1].Dequeue();

                if (p1 > p2)
                {
                    players[0].Enqueue(p1);
                    players[0].Enqueue(p2);
                }
                else
                {
                    players[1].Enqueue(p2);
                    players[1].Enqueue(p1);
                }
            }

            Queue<int> winner = players[0].Count > 0 ? players[0] : players[1];

            return $"{winner.Reverse().Select((a, i) => a * (i + 1)).Sum()}";
        }

        public override string ProcessSecond()
        {
            var players = GetContent()
                .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse))
                .Select(x => new Queue<int>(x))
                .ToList();

            var player1 = new Queue<int>(players[0]);
            var player2 = new Queue<int>(players[1]);

            HashSet<string> rounds = new();
            while (player1.Count > 0 && player2.Count > 0)
            {
                if (rounds.Contains($"{string.Join("-", player1)}|{string.Join("-", player2)}"))
                {
                    break;
                }

                rounds.Add($"{string.Join("-", player1)}|{string.Join("-", player2)}");

                int p1 = player1.Dequeue();
                int p2 = player2.Dequeue();
                if (player1.Count >= p1 && player2.Count >= p2)
                {
                    if (PlayGame(new Queue<int>(player1.Take(p1)), new Queue<int>(player2.Take(p2))))
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                }
                else if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            Queue<int>? winner = player1.Count > 0 ? player1 : player2;

            return $"{winner.Reverse().Select((a, i) => a * (i + 1)).Sum()}";
        }

        private bool PlayGame(Queue<int> player1, Queue<int> player2)
        {
            HashSet<string> rounds = new();
            while (player1.Count > 0 && player2.Count > 0)
            {
                if (rounds.Contains($"{string.Join("-", player1)}|{string.Join("-", player2)}"))
                {
                    return true;
                }

                rounds.Add($"{string.Join("-", player1)}|{string.Join("-", player2)}");

                int p1 = player1.Dequeue();
                int p2 = player2.Dequeue();
                if (player1.Count >= p1 && player2.Count >= p2)
                {
                    if (PlayGame(new Queue<int>(player1.Take(p1)), new Queue<int>(player2.Take(p2))))
                    {
                        player1.Enqueue(p1);
                        player1.Enqueue(p2);
                    }
                    else
                    {
                        player2.Enqueue(p2);
                        player2.Enqueue(p1);
                    }
                }
                else if (p1 > p2)
                {
                    player1.Enqueue(p1);
                    player1.Enqueue(p2);
                }
                else
                {
                    player2.Enqueue(p2);
                    player2.Enqueue(p1);
                }
            }

            return player1.Count > 0;
        }
    }
}
