using System;

namespace Algodat.Stacks
{
    public class LinkedStack<T> : IStack<T>
    {
        private class Node
        {
            public Node Previous { get; set; }
            public T Value { get; set; }
        }

        private Node top;

        public T Peek()
        {
            if (top == null)
            {
                throw new InvalidOperationException();
            }

            return top.Value;
        }

        public T Pop()
        {
            var returnValue = Peek();
            top = top.Previous;
            return returnValue;
        }

        public void Push(T value)
        {
            var newNode = new Node { Value = value };
            if (top != null)
            {
                newNode.Previous = top;
            }
            top = newNode;
        }
    }
}
