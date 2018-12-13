using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
    public class Day13 : Day
    {
        public override string Title => "Mine Cart Madness";

        public override string ProcessFirst()
        {
            var map = GetLinesAsStrings().ToList();
            var carts = new List<Cart>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    int direction = -1;
                    switch (map[i][j])
                    {
                        case '<':
                            direction = 0;
                            break;
                        case '^':
                            direction = 1;
                            break;
                        case '>':
                            direction = 2;
                            break;
                        case 'v':
                            direction = 3;
                            break;
                    }

                    if (direction != -1)
                    {
                        carts.Add(new Cart { X = i, Y = j, Direction = direction, NextIntersectionDirection = 0 });
                    }
                }
            }

            bool hasCrash = false;
            string returnValue = "";
            while (!hasCrash)
            {
                foreach (var cart in carts.OrderBy(c => c.X).ThenBy(c => c.Y))
                {
                    switch (cart.Direction)
                    {
                        case 0:
                            cart.Y -= 1;
                            break;
                        case 1:
                            cart.X -= 1;
                            break;
                        case 2:
                            cart.Y += 1;
                            break;
                        case 3:
                            cart.X += 1;
                            break;
                    }

                    switch (map[cart.X][cart.Y])
                    {
                        case '+':
                            cart.Direction = (cart.Direction + 4 + cart.NextIntersectionDirection - 1) % 4;
                            cart.NextIntersectionDirection = (3 + cart.NextIntersectionDirection + 1) % 3;
                            break;
                        case '\\' when cart.Direction == 0:
                        case '/' when cart.Direction == 3:
                        case '/' when cart.Direction == 1:
                        case '\\' when cart.Direction == 2:
                            cart.Direction = (4 + cart.Direction + 1) % 4;
                            break;
                        case '/' when cart.Direction == 2:
                        case '\\' when cart.Direction == 1:
                        case '/' when cart.Direction == 0:
                        case '\\' when cart.Direction == 3:
                            cart.Direction = (4 + cart.Direction - 1) % 4;
                            break;
                    }

                    var groups = carts.GroupBy(c => (c.X, c.Y));
                    if (groups.Any(g => g.Count() >= 2))
                    {
                        hasCrash = true;
                        var crashCart = groups.First(g => g.Count() >= 2).First();
                        returnValue = $"{crashCart.Y},{crashCart.X}";
                    }
                }
            }

            return returnValue;
        }

        public override string ProcessSecond()
        {
            var map = GetLinesAsStrings().ToList();
            var carts = new List<Cart>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    int direction = -1;
                    switch (map[i][j])
                    {
                        case '<':
                            direction = 0;
                            break;
                        case '^':
                            direction = 1;
                            break;
                        case '>':
                            direction = 2;
                            break;
                        case 'v':
                            direction = 3;
                            break;
                    }

                    if (direction != -1)
                    {
                        carts.Add(new Cart { X = i, Y = j, Direction = direction, NextIntersectionDirection = 0 });
                    }
                }
            }

            while (carts.Count >= 2)
            {
                foreach (var cart in carts.OrderBy(c => c.X).ThenBy(c => c.Y))
                {
                    switch (cart.Direction)
                    {
                        case 0:
                            cart.Y -= 1;
                            break;
                        case 1:
                            cart.X -= 1;
                            break;
                        case 2:
                            cart.Y += 1;
                            break;
                        case 3:
                            cart.X += 1;
                            break;
                    }

                    switch (map[cart.X][cart.Y])
                    {
                        case '+':
                            cart.Direction = (cart.Direction + 4 + cart.NextIntersectionDirection - 1) % 4;
                            cart.NextIntersectionDirection = (3 + cart.NextIntersectionDirection + 1) % 3;
                            break;
                        case '\\' when cart.Direction == 0:
                        case '/' when cart.Direction == 3:
                        case '/' when cart.Direction == 1:
                        case '\\' when cart.Direction == 2:
                            cart.Direction = (4 + cart.Direction + 1) % 4;
                            break;
                        case '/' when cart.Direction == 2:
                        case '\\' when cart.Direction == 1:
                        case '/' when cart.Direction == 0:
                        case '\\' when cart.Direction == 3:
                            cart.Direction = (4 + cart.Direction - 1) % 4;
                            break;
                    }

                    var groups = carts.GroupBy(c => (c.X, c.Y));
                    if (groups.Any(g => g.Count() >= 2))
                    {
                        foreach (var cart1 in groups.Where(x => x.Count() >= 2).SelectMany(g => g))
                        {
                            carts.Remove(cart1);
                        }
                    }
                }
            }

            return $"{carts[0].Y},{carts[0].X}";
        }

        private class Cart
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Direction { get; set; } // 0, 1, 2, 3
            public int NextIntersectionDirection { get; set; } // 0, 1, 2
        }
    }
}