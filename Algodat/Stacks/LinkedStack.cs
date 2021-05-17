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

        private Node _top;

        public T Peek()
        {
            if (_top == null)
            {
                throw new InvalidOperationException();
            }

            return _top.Value;
        }

        public T Pop()
        {
            var returnValue = Peek();
            _top = _top.Previous;
            return returnValue;
        }

        public void Push(T value)
        {
            var newNode = new Node { Value = value };
            if (_top != null)
            {
                newNode.Previous = _top;
            }
            _top = newNode;
        }
    }
}
