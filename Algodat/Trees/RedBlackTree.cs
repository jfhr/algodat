using System;
using System.Collections.Generic;

namespace Algodat.Trees
{
    public class RedBlackTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private class TreeNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public bool IsRed { get; set; }
            
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }

            public TreeNode(TKey key, TValue value, bool isRed = false)
            {
                Key = key;
                Value = value;
                IsRed = isRed;
            }
        }

        /// <summary>
        /// Perform right-rotate on a node and return its right child, i.e. the pivot.
        /// </summary>
        private TreeNode RightRotate(TreeNode root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            var pivot = root.Right;
            if (pivot == null) throw new ArgumentException("Right rotate on node without right child");

            root.Left = pivot.Right;
            pivot.Right = root;
            return pivot;
        }

        /// <summary>
        /// Perform left-rotate on a node and return its left child, i.e. the pivot.
        /// </summary>
        private TreeNode LeftRotate(TreeNode root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            var pivot = root.Left;
            if (pivot == null) throw new ArgumentException("Left rotate on node without right child");

            root.Right = pivot.Left;
            pivot.Left = root;
            return pivot;
        }

        public void Insert(TKey key, TValue value)
        {
            var newNode = new TreeNode(key, value, true);
            throw new NotImplementedException();
        }

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }
        
        public bool Search(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<TKey, TValue> Minimum()
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<TKey, TValue> Maximum()
        {
            throw new NotImplementedException();
        }
    }
}