using System;
using System.Collections.Generic;

namespace Algodat.Trees
{
    public class SimpleBinaryTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        public bool Search(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public void Insert(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<TKey, TValue> Maximum()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<TKey, TValue> Minimum()
        {
            throw new NotImplementedException();
        }
    }
}
