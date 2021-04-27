using Algodat.HashTables;
using System;
using System.Collections.Generic;

namespace Algodat.Trees
{
    // We reuse the IHashTable interface here and add some extra methods.
    // This also allows us to reuse the IHashTable unit tests for ITree implementations.
    public interface ITree<TKey, TValue> : IHashTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Return the minimum key, and its associated value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The tree is empty.
        /// </exception>
        public KeyValuePair<TKey, TValue> Minimum();

        /// <summary>
        /// Return the maximum key, and its associated value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The tree is empty.
        /// </exception>
        public KeyValuePair<TKey, TValue> Maximum();
    }
}
