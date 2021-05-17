namespace Algodat.HashTables
{
    public class OpenAddressingWithLinearProbingHashTable<TKey, TValue> : IHashTable<TKey, TValue> where TValue : class
    {
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public NodeState State { get; set; }

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                State = NodeState.Normal;
            }
        }

        private enum NodeState
        {
            Normal,
            DeleteMe
        }

        private Node[] _array;
        private int _count;

        private double LoadFactor => (double) _count / _array.Length;

        private const double MaxLoadFactor = 0.55;
        private const double LowLoadFactor = 0.2;


        public OpenAddressingWithLinearProbingHashTable()
        {
            _array = new Node[4];
        }

        private int Hash(TKey key, int i) => (key.GetHashCode() + i) % _array.Length;

        /// <summary>
        /// Change array size to a new value and re-insert all items.
        /// </summary>
        private void ChangeArraySize(int newSize)
        {
            var oldArray = _array;
            _array = new Node[newSize];
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
            ChangeArraySize(_array.Length * 2);
        }

        /// <summary>
        /// Half the size of the array and re-insert all elements.
        /// </summary>
        private void ShrinkArray()
        {
            ChangeArraySize(_array.Length / 2);
        }

        /// <summary>
        /// Searches for the key and returns its index. If the key is 
        /// not found, returns the index where it would be inserted.
        /// </summary>
        private int FindIndex(TKey key, bool skipDeleteMe)
        {
            for (int i = 0;; i++)
            {
                int index = Hash(key, i);
                var node = _array[index];
                if (node == null)
                {
                    return index;
                }

                if (Equals(node.Key, key) && (node.State != NodeState.DeleteMe || !skipDeleteMe))
                {
                    return index;
                }
            }
        }

        public void Insert(TKey key, TValue value)
        {
            int index = FindIndex(key, false);

            // If the key is already in the table, just override the value
            if (_array[index] != null)
            {
                _array[index].Value = value;
                _array[index].State = NodeState.Normal;
            }

            // Otherwise, insert a new node
            else
            {
                _array[index] = new Node(key, value);

                _count++;
                if (LoadFactor > MaxLoadFactor)
                {
                    ExpandArray();
                }
            }
        }

        public void Remove(TKey key)
        {
            int index = FindIndex(key, true);

            if (_array[index] != null)
            {
                _array[index].State = NodeState.DeleteMe;

                // Shrink array if load gets low.
                // This isn't strictly necessary, but reduces memory load
                _count--;
                if (LoadFactor < LowLoadFactor)
                {
                    ShrinkArray();
                }
            }
        }

        public TValue Search(TKey key)
        {
            var node = _array[FindIndex(key, true)];
            return node?.Value;
        }
    }
}