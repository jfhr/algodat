namespace Algodat.HashTables
{
    public interface IHashTable<TKey, TValue>
    {
        /// <summary>
        /// Find the value associated with the <paramref name="key"/>.
        /// Return a boolean indicating whether the search was successful.
        /// </summary>
        public bool Search(TKey key, out TValue value);

        /// <summary>
        /// Insert the given key-value-pair.
        /// </summary>
        public void Insert(TKey key, TValue value);

        /// <summary>
        /// Remove the given key, if it exists.
        /// </summary>
        public void Remove(TKey key);
    }
}
