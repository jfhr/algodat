using System;

namespace Algodat.Queues
{
    public class ArrayQueue<T> : IQueue<T>
    {
        private T[] array;

        // index of the first active element
        private int start;

        // index of the last active element
        private int end;

        public ArrayQueue()
        {
            array = Array.Empty<T>();
        }

        private bool IsEmpty => end == start;

        private void IncreaseCapacity()
        {
            if (start == 0)
            {
                // We are already using the full array, so we need a bigger one
                var newLength = array.Length == 0
                    ? 4
                    : array.Length * 2;
                var newArray = new T[newLength];
                Array.Copy(array, newArray, end);
                array = newArray;
            }
            else
            {
                // We are not using the full array, so left-shift to get more space

                // ConstrainedCopy still works if source and destination overlap, see docs
                Array.ConstrainedCopy(array, start, array, 0, end - start);
                end -= start;
                start = 0;
            }
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            T value = array[start];
            start++;
            return value;
        }

        public void Enqueue(T value)
        {
            if (end >= array.Length - 1)
            {
                IncreaseCapacity();
            }
            array[end] = value;
            end++;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }
            return array[start];
        }
    }
}
