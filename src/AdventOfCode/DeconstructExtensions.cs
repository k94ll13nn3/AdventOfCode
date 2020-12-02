using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public static class DeconstructExtensions
    {
        public static void Deconstruct<T>(this IList<T> items, out T t0)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));
            if (items.Count < 1)
            {
                throw new ArgumentException("Number of items must be more than 0.", nameof(items));
            }

            t0 = items[0];
        }

        public static void Deconstruct<T>(this IList<T> items, out T t0, out T t1)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));
            if (items.Count < 2)
            {
                throw new ArgumentException("Number of items must be more than 1.", nameof(items));
            }

            t0 = items[0];
            t1 = items[1];
        }

        public static void Deconstruct<T>(this IList<T> items, out T t0, out T t1, out T t2)
        {
            _ = items ?? throw new ArgumentNullException(nameof(items));
            if (items.Count < 3)
            {
                throw new ArgumentException("Number of items must be more than 1.", nameof(items));
            }

            t0 = items[0];
            t1 = items[1];
            t2 = items[2];
        }
    }
}
