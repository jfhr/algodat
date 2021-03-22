using System;

namespace Algodat
{
    public static class ArrayUtil
    {
        /// <summary>
        /// Swap the elements at index <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        public static void Swap<T>(T[] array, int a, int b)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (a < 0 || a >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(a));
            }
            if (b < 0 || b >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(b));
            }

            if (a == b)
            {
                return;
            }

            T temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}
