using System;

namespace Algodat.Queues
{
    public interface IQueue<T>
    {
        /// <summary>
        /// Add an item to the queue.
        /// </summary>
        void Enqueue(T value);
        
        /// <summary>
        /// Return and remove the next item from the queue
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The queue is empty.
        /// </exception>
        T Dequeue();
        
        /// <summary>
        /// Return the next item from the queue
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The queue is empty.
        /// </exception>
        T Peek();
    }
}
