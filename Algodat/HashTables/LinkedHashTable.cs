namespace Algodat.HashTables
{
    public class LinkedHashTable<TKey, TValue> : IHashTable<TKey, TValue>
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

        private Node[] array;
        private int count;

        private double LoadFactor => count / array.Length;

        private const double MaxLoadFactor = 0.75;
        private const double LowLoadFactor = 0.2;

        public LinkedHashTable()
        {
            array = new Node[4];
        }

        private int Hash(TKey key)
        {
            return (int)((uint)key!.GetHashCode() % (uint)array.Length);
        }

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
        /// Searches for the key and returns its hash, the previous node 
        /// and the containing node.
        /// </summary>
        private (int Hash, Node prevNode, Node Node) FindNode(TKey key)
        {
            int hash = Hash(key);
            var node = array[hash];
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
                array[hash] = new Node(array[hash], key, value);

                count++;
                if (LoadFactor > MaxLoadFactor)
                {
                    ExpandArray();
                }
            }
        }

        public void Remove(TKey key)
        {
            var (hash, prevNode, node) = FindNode(key);

            if (node != null) 
            { 
                if (prevNode == null)
                {
                    array[hash] = node.Next;
                }
                else
                {
                    prevNode.Next = node.Next;
                }

                // Shrink array if load gets low.
                // This isn't strictly necessary, but reduces memory load
                count--;
                if (LoadFactor < LowLoadFactor)
                {
                    ShrinkArray();
                }
            }
        }

        public bool Search(TKey key, out TValue value)
        {
            var (_, _, node) = FindNode(key);

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
