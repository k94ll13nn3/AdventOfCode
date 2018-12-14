using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day14 : Day
    {
        public override string Title => "Chocolate Charts";

        public override string ProcessFirst()
        {
            var input = 380621;
            var recipes = new List<int> { 3, 7 };
            var player1 = 0;
            var player2 = 1;

            while (recipes.Count < input + 10)
            {
                var sum = recipes[player1] + recipes[player2];
                var newRecipe1 = sum % 10;
                var newRecipe2 = (sum / 10) % 10;
                if (newRecipe2 != 0)
                {
                    recipes.Add(newRecipe2);
                };
                recipes.Add(newRecipe1);

                player1 = (player1 + 1 + recipes[player1]) % recipes.Count;
                player2 = (player2 + 1 + recipes[player2]) % recipes.Count;
            }

            return recipes.Skip(input).Take(10).Aggregate(string.Empty, (s, i) => s + i);
        }

        public override string ProcessSecond()
        {
            var input = "380621".AsSpan();
            Span<char> recipes = new char[2000];
            recipes.Fill('!');
            "37".AsSpan().CopyTo(recipes);
            var player1 = 0;
            var player2 = 1;
            var currentIndex = 2;

            // no more than +2 new recipe by turn, input length is 6, so it can only be in the last 8 (10 to be sure)
            while (recipes.Slice(Math.Max(0, currentIndex - 10), 10).IndexOf(input) == -1)
            {
                if (currentIndex > recipes.Length - 2)
                {
                    Span<char> s = new char[(int)(recipes.Length * 1.5)];
                    s.Fill('!');
                    recipes.CopyTo(s);
                    recipes = s;
                }

                var sum = (recipes[player1] - '0') + (recipes[player2] - '0');
                var newRecipe1 = sum % 10;
                var newRecipe2 = (sum / 10) % 10;
                if (newRecipe2 != 0)
                {
                    recipes[currentIndex] = (char)(newRecipe2 + '0');
                    currentIndex++;
                };
                recipes[currentIndex] = (char)(newRecipe1 + '0');
                currentIndex++;

                player1 = (player1 + 1 + (recipes[player1] - '0')) % currentIndex;
                player2 = (player2 + 1 + (recipes[player2] - '0')) % currentIndex;
            }

            return (recipes.IndexOf(input)).ToString();
        }
    }
}