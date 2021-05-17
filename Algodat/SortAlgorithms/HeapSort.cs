namespace Algodat.SortAlgorithms
{
    public class HeapSort : ISortAlgorithm
    {
        public void SortAscending(int[] array)
        {
            CreateInstance(array).SortAscending();
        }

        private static Instance CreateInstance(int[] array) => new(array);

        private class Instance
        {
            private readonly int[] _array;

            /// <summary>
            /// Size of the heap, this is also the smallest index in the array
            /// that is not itself part of the heap.
            /// </summary>
            private int _heapSize;

            public Instance(int[] array)
            {
                this._array = array;
                _heapSize = array.Length;
            }

            public void SortAscending()
            {
                BuildMaxHeap();

                while (_heapSize > 1)
                {
                    Swap(0, _heapSize - 1);
                    _heapSize--;
                    MaxHeapify(0);
                }
            }

            /// <summary>
            /// Turn an arbitrary array into a complete max-heap.
            /// </summary>
            private void BuildMaxHeap()
            {
                for (int i = _array.Length / 2; i >= 0; i--)
                {
                    MaxHeapify(i);
                }
            }

            /// <summary>
            /// Ensure that the item at the given index satisfies the max-heap property,
            /// given that its children already satisfy it.
            /// </summary>
            private void MaxHeapify(int index)
            {
                int left = index * 2 + 1;
                if (left >= _heapSize)
                {
                    return;
                }

                int right = left + 1;
                if (right >= _heapSize || _array[left] >= _array[right])
                {
                    if (_array[index] < _array[left])
                    {
                        Swap(index, left);
                        MaxHeapify(left);
                    }
                }
                else
                {
                    if (_array[index] < _array[right])
                    {
                        Swap(index, right);
                        MaxHeapify(right);
                    }
                }
            }

            private void Swap(int a, int b) => ArrayUtil.Swap(_array, a, b);
        }
    }
}
