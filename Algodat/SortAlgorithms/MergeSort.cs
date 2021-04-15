using System;

namespace Algodat.SortAlgorithms
{
    public class MergeSort : ISortAlgorithm
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

            int middle = span.Length / 2;
            var left = span[..middle];
            var right = span[middle..];

            SortInternal(left);
            SortInternal(right);
            Merge(span, left, right);
        }

        private void Merge(Span<int> destination, Span<int> left, Span<int> right)
        {
            var leftCopy = Copy(left);
            int leftIndex = 0;

            var rightCopy = Copy(right);
            int rightIndex = 0;

            for (int i = 0; i < destination.Length; i++)
            {
                if (leftIndex >= leftCopy.Length)
                {
                    destination[i] = rightCopy[rightIndex];
                    rightIndex++;
                }
                else if (rightIndex >= rightCopy.Length || rightCopy[rightIndex] > leftCopy[leftIndex])
                {
                    destination[i] = leftCopy[leftIndex];
                    leftIndex++;
                }
                else
                {
                    destination[i] = rightCopy[rightIndex];
                    rightIndex++;
                }
            }
        }

        private int[] Copy(Span<int> span)
        {
            var copy = new int[span.Length];
            span.CopyTo(copy);
            return copy;
        }
    }
}
