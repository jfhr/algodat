using System;

namespace Algodat.Stacks
{
    public class ArrayStack<T> : IStack<T>
    {
        private T[] _array;

        // index of the last active element
        private int _end;

        public ArrayStack()
        {
            _array = Array.Empty<T>();
            _end = -1;
        }

        private bool IsEmpty => _end == -1;

        private void IncreaseCapacity()
        {
            // We are already using the full array, so we need a bigger one
            var newLength = _array.Length == 0
                ? 4
                : _array.Length * 2;
            var newArray = new T[newLength];
            Array.Copy(_array, newArray, _end);
            _array = newArray;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }
            return _array[_end];
        }

        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            T value = _array[_end];
            _end--;
            return value;
        }

        public void Push(T value)
        {
            _end++;
            if (_end >= _array.Length)
            {
                IncreaseCapacity();
            }
            _array[_end] = value;
        }
    }
}
