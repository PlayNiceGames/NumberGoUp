using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
                .Range(1, (1 << data.Length) - 1)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0));
        }

        public static T MaxBy<T, R>(this IEnumerable<T> source, Func<T, R> comparator) where R : IComparable<R>
        {
            return source.Select(t => new Tuple<T, R>(t, comparator(t)))
                .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) > 0 ? next : max).Item1;
        }

        public static T MinBy<T, R>(this IEnumerable<T> source, Func<T, R> evaluate) where R : IComparable<R>
        {
            return source.Select(t => new Tuple<T, R>(t, evaluate(t)))
                .Aggregate((max, next) => next.Item2.CompareTo(max.Item2) < 0 ? next : max).Item1;
        }
    }
}