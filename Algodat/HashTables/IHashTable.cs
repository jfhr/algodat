using System.Diagnostics.CodeAnalysis;

namespace Algodat.HashTables
{
    public interface IHashTable<TKey, TValue>
    {
        /// <summary>
        /// Return the value associated with the <paramref name="key"/>, or 
        /// <see langword="null"/> if it is not found.
        /// </summary>
        public TValue? Search([NotNull] TKey key);

        /// <summary>
        /// Insert the given key-value-pair.
        /// </summary>
        public void Insert([NotNull] TKey key, [MaybeNull] TValue value);

        /// <summary>
        /// Remove the given key, if it exists.
        /// </summary>
        public void Remove([NotNull] TKey key);
    }
}
