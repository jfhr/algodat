namespace Algodat.HashTables
{
    // Double hashing on generic values doesn't really work because generic values
    // only have one GetHashCode() function.
    // 
    // For the sake of simplicity, we restrict the keys to integers.
    // Other data types could be implemented by providing a second hash function that
    // is pairwise independent of GetHashCode().
    public class OpenAddressingWithDoubleHashingHashTable<TValue> : IHashTable<int, TValue>
    {
        private class Node
        {
            public int Key { get; set; }
            public TValue Value { get; set; }
            public NodeState State { get; set; }

            public Node(int key, TValue value)
            {
                Key = key;
                Value = value;
                State = NodeState.Normal;
            }
        }

        private enum NodeState { Normal, DeleteMe }

        private Node[] array;
        private int count;

        private double LoadFactor => count / array.Length;

        private const double MaxLoadFactor = 0.55;
        private const double LowLoadFactor = 0.2;


        public OpenAddressingWithDoubleHashingHashTable()
        {
            array = new Node[4];
        }

        private int Hash(int key, int i) => H1(key) + i * H2(key);

        private int H1(int key) => key % array.Length;

        private int H2(int key) => (key % (array.Length - 1)) + 1;

        /// <summary>
        /// Change array size to a new value and re-insert all items.
        /// </summary>
        private void ChangeArraySize(int newSize)
        {
            var oldArray = array;
            array = new Node[newSize];
            for (int i = 0; i < oldArray.Length; i++)
            {
                var node = oldArray[i];
                if (node != null && node.State != NodeState.DeleteMe)
                {
                    Insert(node.Key, node.Value);
                }
            }
        }

        /// <summary>
        /// Double the size of the array and re-insert all elements.
        /// </summary>
        private void ExpandArray()
        {
            ChangeArraySize(array.Length * 2);
        }

        /// <summary>
        /// Half the size of the array and re-insert all elements.
        /// </summary>
        private void ShrinkArray()
        {
            ChangeArraySize(array.Length / 2);
        }

        /// <summary>
        /// Searches for the key and returns its index. If the key is 
        /// not found, returns the index where it would be inserted.
        /// </summary>
        private int FindIndex(int key, bool skipDeleteMe)
        {
            for (int i = 0; ; i++)
            {
                int index = Hash(key, i);
                var node = array[index];
                if (node == null)
                {
                    return index;
                }
                if (node.Key == key && (node.State != NodeState.DeleteMe || !skipDeleteMe))
                {
                    return index;
                }
            }
        }

        public void Insert(int key, TValue value)
        {
            int index = FindIndex(key, false);

            // If the key is already in the table, just override the value
            if (array[index] != null)
            {
                array[index].Value = value;
                array[index].State = NodeState.Normal;
            }

            // Otherwise, insert a new node
            else
            {
                array[index] = new Node(key, value);

                count++;
                if (LoadFactor > MaxLoadFactor)
                {
                    ExpandArray();
                }
            }
        }

        public void Remove(int key)
        {
            int index = FindIndex(key, true);

            if (array[index] != null)
            {
                array[index].State = NodeState.DeleteMe;

                // Shrink array if load gets low.
                // This isn't strictly necessary, but reduces memory load
                count--;
                if (LoadFactor < LowLoadFactor)
                {
                    ShrinkArray();
                }
            }
        }

        public bool Search(int key, out TValue value)
        {
            var node = array[FindIndex(key, true)];

            if (node == null)
            {
                value = default;
                return false;
            }
            else
            {
                value = node.Value;
                return true;
            }
        }
    }
}
