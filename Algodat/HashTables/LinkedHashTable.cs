namespace Algodat.HashTables
{
    public class LinkedHashTable<TKey, TValue> : IHashTable<TKey, TValue> where TValue : class
    {
        private class Node
        {
            public Node Next { get; set; }
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public Node(Node next, TKey key, TValue value)
            {
                Next = next;
                Key = key;
                Value = value;
            }
        }

        private Node[] _array;
        private int _count;

        private double LoadFactor => _count / _array.Length;

        private const double MaxLoadFactor = 0.75;
        private const double LowLoadFactor = 0.2;

        public LinkedHashTable()
        {
            _array = new Node[4];
        }

        private int Hash(TKey key)
        {
            return (int)((uint)key!.GetHashCode() % (uint)_array.Length);
        }

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
                while (node != null)
                {
                    Insert(node.Key, node.Value);
                    node = node.Next;
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
        /// Searches for the key and returns its hash, the previous node 
        /// and the containing node.
        /// </summary>
        private (int Hash, Node prevNode, Node Node) FindNode(TKey key)
        {
            int hash = Hash(key);
            var node = _array[hash];
            Node prevNode = null;
            while (node != null && !Equals(key, node.Key))
            {
                prevNode = node;
                node = node.Next;
            }
            return (hash, prevNode, node);
        }

        public void Insert(TKey key, TValue value)
        {
            var (hash, _, node) = FindNode(key);

            // If the key is already in the table, just override the value
            if (node != null)
            {
                node.Value = value;
            }

            // Otherwise, insert a new node
            else
            {
                _array[hash] = new Node(_array[hash], key, value);

                _count++;
                if (LoadFactor > MaxLoadFactor)
                {
                    ExpandArray();
                }
            }
        }

        public void Remove(TKey key)
        {
            var (hash, prevNode, node) = FindNode(key);
            if (node == null)
            {
                return;
            }
            
            if (prevNode == null)
            {
                _array[hash] = node.Next;
            }
            else
            {
                prevNode.Next = node.Next;
            }

            // Shrink array if load gets low.
            // This isn't strictly necessary, but reduces memory load
            _count--;
            if (LoadFactor < LowLoadFactor)
            {
                ShrinkArray();
            }
        }

        public TValue Search(TKey key)
        {
            var (_, _, node) = FindNode(key);
            return node?.Value;
        }
    }
}
