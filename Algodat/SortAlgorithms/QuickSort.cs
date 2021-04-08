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

            int pivot = span[^1];
            int i = -1;
            for (int j = 0; j < span.Length; j++)
            {
                if (span[j] <= pivot)
                {
                    ArrayUtil.Swap(span, i + 1, j);
                    i++;
                }
            }

            var left = span[..i];
            var right = span[(i + 1)..];

            SortInternal(left);
            SortInternal(right);
        }
    }
}
