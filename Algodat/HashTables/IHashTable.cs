namespace Algodat.HashTables
{
    public interface IHashTable<in TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Return the value associated with the <paramref name="key"/>,
        /// or <see langword="null"/> if it was not found.
        /// </summary>
        public TValue Search(TKey key);

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
