using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
    public class Day9 : Day
    {
        public override string Title => "Marble Mania";

        public override string ProcessFirst()
        {
            return Compute(71082).ToString();
        }

        public override string ProcessSecond()
        {
            return Compute(71082 * 100).ToString();
        }

        public ulong Compute(int n)
        {
            var players = 413;
            var lastMarble = n;

            var marbles = new LinkedList<uint>();
            var node = marbles.AddFirst(0);
            var currentMarble = 0;
            var currentPlayer = 0;
            var scores = new ulong[players];

            for (uint i = 1; i <= lastMarble; i++)
            {
                currentPlayer = (currentPlayer + 1) % players;

                if (marbles.Count < 2)
                {
                    node = marbles.AddLast(i);
                }
                else if (i % 23 == 0)
                {
                    currentMarble = (marbles.Count + currentMarble - 7) % marbles.Count;
                    var e = node.Previous().Previous().Previous().Previous().Previous().Previous().Previous();
                    scores[currentPlayer] += i;
                    scores[currentPlayer] += e.Value;
                    node = e.Next ?? marbles.First;
                    marbles.Remove(e);
                }
                else
                {
                    currentMarble = (currentMarble + 2);
                    if (currentMarble == marbles.Count)
                    {
                        node = marbles.AddLast(i);
                    }
                    else
                    {
                        currentMarble = currentMarble % marbles.Count;
                        node = marbles.AddAfter(node.Next ?? marbles.First, i);
                    }
                }

            }

            return scores.Max();
        }
    }

    public static class E { public static LinkedListNode<T> Previous<T>(this LinkedListNode<T> node) => node.Previous ?? node.List.Last; }
}