using System;
using System.Diagnostics;

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

        private void SortInternal(Span<int> span, int bitIndex)
        {
            if (span.Length <= 1)
            {
                return;
            }

            int i0 = 0;
            int i1 = span.Length - 1;

            for (int i = 0; i0 <= i1; i = i0)
            {
                // Because ints are signed, the sign bit (bitIndex == 31)
                // must be treated opposite to the other bits
                if (bitIndex == 31 != IsBitSet(span[i], bitIndex))
                {
                    ArrayUtil.Swap(span, i, i1);
                    i1--;
                }
                else
                {
                    i0++;
                }
            }

            if (bitIndex > 0)
            {
                SortInternal(span[..i0], bitIndex - 1);
                SortInternal(span[i0..], bitIndex - 1);
            }
        }

        private bool IsBitSet(int value, int bitIndex)
        {
            return ((value >> bitIndex) & 1) == 1;
        }
    }
}
