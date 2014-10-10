﻿using System;
using System.Collections.Generic;

namespace GeodesicGrid.EnumerableExtensions
{
    public static class AppendPrepend
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T element)
        {
            return new AppendIterator<T>(sequence, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> sequence, T element)
        {
            return new PrependIterator<T>(sequence, element);
        }

        private sealed class AppendIterator<T> : AppendPrependIterator<T>
        {
            public AppendIterator(IEnumerable<T> sequence, T element) : base(sequence, element) { }
        }

        private sealed class PrependIterator<T> : AppendPrependIterator<T>
        {
            public PrependIterator(IEnumerable<T> sequence, T element) : base(sequence, element) { }
        }

        private abstract class AppendPrependIterator<T> : IEnumerable<T>
        {
            private readonly T element;
            private readonly IEnumerable<T> sequence;

            protected AppendPrependIterator(IEnumerable<T> sequence, T element)
            {
                if (sequence == null) { throw new ArgumentNullException("sequence"); }
                this.element = element;
                this.sequence = sequence;
            }

            public IEnumerator<T> GetEnumerator()
            {
                var appends = new Stack<T>();
                var iterator = this;
                IEnumerable<T> seq;

                do
                {
                    if (iterator is AppendIterator<T>)
                    {
                        appends.Push(iterator.element);
                    }
                    else // (iterator is PrependIterator<T>)
                    {
                        yield return iterator.element;
                    }
                    seq = iterator.sequence;
                    iterator = seq as AppendPrependIterator<T>;
                } while (iterator != null);

                foreach (var e in seq) { yield return e; }
                while (appends.Count > 0) { yield return appends.Pop(); }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
