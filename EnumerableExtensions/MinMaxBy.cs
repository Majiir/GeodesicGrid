using System;
using System.Collections.Generic;

namespace GeodesicGrid.EnumerableExtensions
{
    public static class MinMaxBy
    {
        public static T MinBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        {
            return minByImpl(sequence, selector, Comparer<TKey>.Default, true);
        }

        public static T MaxBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        {
            return minByImpl(sequence, selector, new ReverseComparer<TKey>(Comparer<TKey>.Default), true);
        }

        public static T MinBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            if (comparer == null) { throw new ArgumentNullException("comparer"); }
            return minByImpl(sequence, selector, comparer, true);
        }

        public static T MaxBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            if (comparer == null) { throw new ArgumentNullException("comparer"); }
            return minByImpl(sequence, selector, new ReverseComparer<TKey>(comparer), true);
        }

        public static T MinByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        {
            return minByImpl(sequence, selector, Comparer<TKey>.Default, false);
        }

        public static T MaxByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector)
        {
            return minByImpl(sequence, selector, new ReverseComparer<TKey>(Comparer<TKey>.Default), false);
        }

        public static T MinByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            if (comparer == null) { throw new ArgumentNullException("comparer"); }
            return minByImpl(sequence, selector, comparer, false);
        }

        public static T MaxByOrDefault<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            if (comparer == null) { throw new ArgumentNullException("comparer"); }
            return minByImpl(sequence, selector, new ReverseComparer<TKey>(comparer), false);
        }

        private static T minByImpl<T, TKey>(IEnumerable<T> sequence, Func<T, TKey> selector, IComparer<TKey> comparer, bool throwOnEmpty)
        {
            if (sequence == null) { throw new ArgumentNullException("sequence"); }
            if (selector == null) { throw new ArgumentNullException("selector"); }

            using (var enumerator = sequence.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    if (throwOnEmpty) { throw new InvalidOperationException("Sequence is empty"); }
                    return default(T);
                }

                T min = enumerator.Current;
                TKey minKey = selector(min);

                while (enumerator.MoveNext())
                {
                    T current = enumerator.Current;
                    TKey currentKey = selector(current);
                    if (comparer.Compare(currentKey, minKey) < 0)
                    {
                        min = current;
                        minKey = currentKey;
                    }
                }

                return min;
            }
        }
    }
}
