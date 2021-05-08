using System;

namespace Algodat.SortAlgorithms
{
    public class QuickSort : ISortAlgorithm
    {
        public void SortAscending(int[] array)
        {
            SortInternal(array);
        }

        private void SortInternal(Span<int> span)
        {
            if (span.Length <= 1)
            {
                return;
            }

            // Choose an arbitrary array element as the pivot.
            // For simplicity, we always use the last element.
            // Some authors suggest using a random element instead
            int pivot = span[^1];
            
            // We divide the array in two buckets: ( < pivot) and ( >= pivot)
            // i is the first index of the second bucket.
            int i = 0;
            
            for (int j = 0; j < span.Length; j++)
            {
                if (span[j] <= pivot)
                {
                    ArrayUtil.Swap(span, i, j);
                    i++;
                }
            }

            // Recursively sort both buckets
            var left = span[..(i-1)];
            var right = span[i..];

            SortInternal(left);
            SortInternal(right);
        }
    }
}
