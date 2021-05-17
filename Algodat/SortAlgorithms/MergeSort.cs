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

            // Divide the array into two halves. We do this recursively
            // until the halves are no more than two elements large.
            int middle = span.Length / 2;
            var left = span[..middle];
            var right = span[middle..];

            SortInternal(left);
            SortInternal(right);
            // This is where the magic happens.
            Merge(span, left, right);
        }

        private void Merge(Span<int> destination, Span<int> left, Span<int> right)
        {
            // In our implementation, left, right and destination are spans over the same physical array.
            // To avoid overwriting destination, we first have to copy left and right.
            var leftCopy = Copy(left);
            int leftIndex = 0;

            var rightCopy = Copy(right);
            int rightIndex = 0;

            // We fill destination by choosing the next element from left or right, whichever is smaller.
            
            // Note: this code is complicated by the fact that we have to check if left or right are already exhausted.
            // It could be simplified by using a sentinel value that is larger than all allowed values.
            // E.g. if we were sorting floats, we could use float.PositiveInfinity
            
            for (int i = 0; i < destination.Length; i++)
            {
                // If left has already been exhausted, choose from right
                if (leftIndex >= leftCopy.Length)
                {
                    destination[i] = rightCopy[rightIndex];
                    rightIndex++;
                }
                // If right has been exhausted, or left < right, choose left
                else if (rightIndex >= rightCopy.Length || rightCopy[rightIndex] > leftCopy[leftIndex])
                {
                    destination[i] = leftCopy[leftIndex];
                    leftIndex++;
                }
                // Otherwise choose right
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
