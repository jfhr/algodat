using System;

namespace Algodat
{
    public static class ArrayUtil
    {
        /// <summary>
        /// Swap the elements at index <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        public static void Swap<T>(T[] array, int a, int b) => Swap(array.AsSpan(), a, b);

        /// <summary>
        /// Swap the elements at index <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        public static void Swap<T>(Span<T> span, int a, int b)
        {
            if (a < 0 || a >= span.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }
            if (b < 0 || b >= span.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            if (a == b)
            {
                return;
            }

            T temp = span[a];
            span[a] = span[b];
            span[b] = temp;
        }
    }
}
