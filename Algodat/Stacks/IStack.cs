using System;

namespace Algodat.Stacks
{
    public interface IStack<T>
    {
        /// <summary>
        /// Add an item to the stack.
        /// </summary>
        void Push(T value);
        
        /// <summary>
        /// Return and remove the next item from the stack.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The stack is empty.
        /// </exception>
        T Pop();
        
        /// <summary>
        /// Return the next item from the stack.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The stack is empty.
        /// </exception>
        T Peek();
    }
}
