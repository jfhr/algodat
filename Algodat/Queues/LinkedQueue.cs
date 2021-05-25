using System;

namespace Algodat.Queues
{
    public class LinkedQueue<T> : IQueue<T>
    {
        private class Node
        {
            public Node Next { get; set; }
            public Node Previous { get; set; }
            public T Value { get; set; }
        }

        private Node _start;
        private Node _end;

        public T Dequeue()
        {
            var returnValue = Peek();
            _end = _end.Previous;
            return returnValue;
        }

        public void Enqueue(T value)
        {
            var newNode = new Node { Value = value };
            if (_start != null)
            {
                newNode.Next = _start;
                _start.Previous = newNode;
            }
            _start = newNode;
            _end ??= newNode;
        }

        public T Peek()
        {
            if (_end is null)
            {
                throw new InvalidOperationException();
            }

            return _end.Value;
        }
    }
}
