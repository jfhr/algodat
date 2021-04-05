using System;

namespace Algodat.Stacks
{
    public class ArrayStack<T> : IStack<T>
    {
        private T[] array;

        // index of the last active element
        private int end;

        public ArrayStack()
        {
            array = Array.Empty<T>();
            end = -1;
        }

        private bool IsEmpty => end == -1;

        private void IncreaseCapacity()
        {
            // We are already using the full array, so we need a bigger one
            var newLength = array.Length == 0
                ? 4
                : array.Length * 2;
            var newArray = new T[newLength];
            Array.Copy(array, newArray, end);
            array = newArray;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }
            return array[end];
        }

        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            T value = array[end];
            end--;
            return value;
        }

        public void Push(T value)
        {
            end++;
            if (end >= array.Length)
            {
                IncreaseCapacity();
            }
            array[end] = value;
        }
    }
}
