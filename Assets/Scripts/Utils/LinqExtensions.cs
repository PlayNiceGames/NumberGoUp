using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
                .Range(1, (1 << data.Length) - 1)
                .Select(index => data
                    .Where((v, i) => (index & (1 << i)) != 0)
                    .ToArray());
        }
    }
}