using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day21 : Day
    {
        public override string Title => "Allergen Assessment";

        public override string ProcessFirst()
        {
            IEnumerable<(string[] ingredients, string[] allergens)> input = GetLines().Select(x =>
            {
                (string ingredientString, string allergenString) = x.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                return (ingredients: ingredientString.Split(' ', StringSplitOptions.RemoveEmptyEntries), allergens: allergenString[8..].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
            });

            var possibilities = new Dictionary<string, HashSet<string>>();
            var count = new Dictionary<string, int>();
            foreach ((string[] ingredients, string[] allergens) in input)
            {
                foreach (string allergen in allergens)
                {
                    if (possibilities.ContainsKey(allergen))
                    {
                        possibilities[allergen].IntersectWith(ingredients);
                    }
                    else
                    {
                        possibilities[allergen] = new HashSet<string>(ingredients);
                    }
                }
            }

            foreach ((string[] ingredients, _) in input)
            {
                foreach (string ingredient in ingredients)
                {
                    if (count.ContainsKey(ingredient))
                    {
                        count[ingredient]++;
                    }
                    else
                    {
                        count[ingredient] = 1;
                    }
                }
            }

            var ingredientWithAllergen = new List<(string ingredient, string allergen)>();
            while (possibilities.Count > 0)
            {
                (string foundAllergen, HashSet<string> ingredient) = possibilities.First(x => x.Value.Count == 1);
                ingredientWithAllergen.Add((ingredient.First(), foundAllergen));
                possibilities.Remove(foundAllergen);
                foreach (KeyValuePair<string, HashSet<string>> possibility in possibilities)
                {
                    possibility.Value.Remove(ingredient.First());
                }
            }

            return $"{count.Where(x => !ingredientWithAllergen.Select(x => x.ingredient).Contains(x.Key)).Sum(x => x.Value)}";
        }

        public override string ProcessSecond()
        {
            IEnumerable<(string[] ingredients, string[] allergens)> input = GetLines().Select(x =>
            {
                (string ingredientString, string allergenString) = x.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                return (ingredients: ingredientString.Split(' ', StringSplitOptions.RemoveEmptyEntries), allergens: allergenString[8..].Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
            });

            var possibilities = new Dictionary<string, HashSet<string>>();
            var count = new Dictionary<string, int>();
            foreach ((string[] ingredients, string[] allergens) in input)
            {
                foreach (string allergen in allergens)
                {
                    if (possibilities.ContainsKey(allergen))
                    {
                        possibilities[allergen].IntersectWith(ingredients);
                    }
                    else
                    {
                        possibilities[allergen] = new HashSet<string>(ingredients);
                    }
                }
            }

            foreach ((string[] ingredients, _) in input)
            {
                foreach (string ingredient in ingredients)
                {
                    if (count.ContainsKey(ingredient))
                    {
                        count[ingredient]++;
                    }
                    else
                    {
                        count[ingredient] = 1;
                    }
                }
            }

            var ingredientWithAllergen = new List<(string ingredient, string allergen)>();
            while (possibilities.Count > 0)
            {
                (string foundAllergen, HashSet<string> ingredient) = possibilities.First(x => x.Value.Count == 1);
                ingredientWithAllergen.Add((ingredient.First(), foundAllergen));
                possibilities.Remove(foundAllergen);
                foreach (KeyValuePair<string, HashSet<string>> possibility in possibilities)
                {
                    possibility.Value.Remove(ingredient.First());
                }
            }

            return $"{string.Join(",", ingredientWithAllergen.OrderBy(x => x.allergen).Select(x => x.ingredient))}";
        }
    }
}
