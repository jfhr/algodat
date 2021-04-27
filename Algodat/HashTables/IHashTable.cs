namespace Algodat.HashTables
{
    public interface IHashTable<TKey, TValue>
    {
        /// <summary>
        /// Find the value associated with the <paramref name="key"/>.
        /// If the search is successful, returns true and sets the out
        /// parameter to the associated value.
        /// Otherwise, returns false, in that case, the out parameter
        /// may be set to any value.
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
