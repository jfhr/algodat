using System;

namespace Algodat.SortAlgorithms
{
    /// <summary>
    /// In-place MSD radix sort
    /// </summary>
    public class RadixSort : ISortAlgorithm
    {
        // bitIndex = 0 is the least significant bit
        // bitIndex = 31 is the most significant bit
        public void SortAscending(int[] array) => SortInternal(array.AsSpan(), 31);

        private static void SortInternal(Span<int> span, int bitIndex)
        {
            if (span.Length <= 1)
            {
                return;
            }

            // We divide the array in two buckets depending on whether our bit is set or not.
            // Smaller numbers go left, larger numbers go right.
            // Here, i0 is the index *after* the end of the left bucket, 
            // and i1 is the index *before* the start of the right bucket.
            int i0 = 0;
            int i1 = span.Length - 1;

            while (i0 <= i1)
            {
                // Because ints are signed, the sign bit (bitIndex == 31)
                // must be treated opposite to the other bits
                if (bitIndex == 31 != IsBitSet(span[i0], bitIndex))
                {
                    ArrayUtil.Swap(span, i0, i1);
                    i1--;
                }
                else
                {
                    i0++;
                }
            }

            // Recursively do this for all 32 bits.
            if (bitIndex > 0)
            {
                SortInternal(span[..i0], bitIndex - 1);
                SortInternal(span[i0..], bitIndex - 1);
            }
        }

        private static bool IsBitSet(int value, int bitIndex)
        {
            return ((value >> bitIndex) & 1) == 1;
        }
    }
}