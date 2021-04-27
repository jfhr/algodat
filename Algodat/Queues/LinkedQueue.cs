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

        private Node start;
        private Node end;

        public T Dequeue()
        {
            var returnValue = Peek();
            end = end.Previous;
            return returnValue;
        }

        public void Enqueue(T value)
        {
            var newNode = new Node { Value = value };
            if (start != null)
            {
                newNode.Next = start;
                start.Previous = newNode;
            }
            start = newNode;
            end ??= newNode;
        }

        public T Peek()
        {
            if (end is null)
            {
                throw new InvalidOperationException();
            }

            return end.Value;
        }
    }
}
