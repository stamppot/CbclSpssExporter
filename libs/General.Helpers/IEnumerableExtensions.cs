using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Helpers
{
    public static class IEnumerableExtensions
    {
        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _lambdaComparer;
            private readonly Func<T, int> _lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer) :
                this(lambdaComparer, o => 0) {
            }

            private LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash) {
                if (lambdaComparer == null)
                    throw new ArgumentNullException("lambdaComparer");
                if (lambdaHash == null)
                    throw new ArgumentNullException("lambdaHash");
                _lambdaComparer = lambdaComparer;
                _lambdaHash = lambdaHash;
            }
            public bool Equals(T x, T y) {
                return _lambdaComparer(x, y);
            }
            public int GetHashCode(T obj) {
                return _lambdaHash(obj);
            }
        }

        public static String JoinStr<T>(this IEnumerable<T> values) {
            return String.Join(", ", values.Select(val => val.ToString()).ToArray());
        }

        public static String JoinStr<T>(this IEnumerable<T> values, String separator) {
            return String.Join(separator, values.Select(val => val.ToString()).ToArray());
        }

        public static String JoinStr<T>(this IEnumerable<T> values, Func<T,String> projection) {
            return String.Join(", ", values.Select(val => projection(val)).ToArray());
        }

        public static IEnumerable<T> Distinct<T, TId>(this IEnumerable<T> values, Func<T, TId> projection)
            where TId : IEquatable<TId> {
            return values.Distinct(new ProjectingEqualityComparer<T, TId>(projection));
        }

        class ProjectingEqualityComparer<T, TId> : IEqualityComparer<T>
            where TId : IEquatable<TId>
        {
            readonly Func<T, TId> projection;

            public ProjectingEqualityComparer(Func<T, TId> projection) {
                this.projection = projection;
            }

            public bool Equals(T x, T y) {
                return EqualityComparer<TId>.Default.Equals(projection(x), projection(y));
            }

            public int GetHashCode(T obj) {
                return EqualityComparer<TId>.Default.GetHashCode(projection(obj));
            }
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> enumerable, TSource value, Func<TSource, TSource, bool> comparer) {
            return enumerable.Contains(value, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer) {
            return enumerable.Intersect(second, new LambdaComparer<TSource>(comparer));
        }

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer) {
            return enumerable.Except(second, new LambdaComparer<TSource>(comparer));
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) {
            foreach (T item in enumeration) action(item);
        }
    }
}
