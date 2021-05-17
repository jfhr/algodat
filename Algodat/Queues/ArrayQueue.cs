using System;

namespace Algodat.Queues
{
    public class ArrayQueue<T> : IQueue<T>
    {
        private T[] _array;

        // index of the first active element
        private int _start;

        // index of the last active element
        private int _end;

        public ArrayQueue()
        {
            _array = Array.Empty<T>();
        }

        private bool IsEmpty => _end == _start;

        private void IncreaseCapacity()
        {
            if (_start == 0)
            {
                // We are already using the full array, so we need a bigger one
                var newLength = _array.Length == 0
                    ? 4
                    : _array.Length * 2;
                var newArray = new T[newLength];
                Array.Copy(_array, newArray, _end);
                _array = newArray;
            }
            else
            {
                // We are not using the full array, so left-shift to get more space

                // ConstrainedCopy still works if source and destination overlap, see docs
                Array.ConstrainedCopy(_array, _start, _array, 0, _end - _start);
                _end -= _start;
                _start = 0;
            }
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            T value = _array[_start];
            _start++;
            return value;
        }

        public void Enqueue(T value)
        {
            if (_end >= _array.Length - 1)
            {
                IncreaseCapacity();
            }
            _array[_end] = value;
            _end++;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }
            return _array[_start];
        }
    }
}
