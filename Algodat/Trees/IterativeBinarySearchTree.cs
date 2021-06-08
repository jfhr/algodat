using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Algodat.Trees
{
    /// <summary>
    /// Iterative variant.
    /// </summary>
    public class IterativeBinarySearchTree<TKey, TValue> : ITree<TKey, TValue>
        where TKey : IComparable<TKey> where TValue : class
    {
        private class TreeNode
        {
            public TKey Key { get; }
            public TValue Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }

            public TreeNode(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        private TreeNode root;

        private bool IsEmpty => root == null;

        public TValue Search(TKey key)
        {
            var current = root;
            while (current != null)
            {
                switch (key.CompareTo(current.Key))
                {
                    case < 0:
                        current = current.Left;
                        break;
                    case 0:
                        return current.Value;
                    case > 0:
                        current = current.Right;
                        break;
                }
            }

            return null;
        }

        public void Insert(TKey key, TValue value)
        {
            if (IsEmpty)
            {
                root = new TreeNode(key, value);
            }
            else
            {
                var current = root;
                while (true)
                {
                    switch (key.CompareTo(current.Key))
                    {
                        case 0:
                            current.Value = value;
                            return;
                        case < 0 when current.Left == null:
                            current.Left = new TreeNode(key, value);
                            return;
                        case < 0:
                            current = current.Left;
                            break;
                        case > 0 when current.Right == null:
                            current.Right = new TreeNode(key, value);
                            return;
                        case > 0:
                            current = current.Right;
                            break;
                    }
                }
            }
        }

        private TreeNode Heir(TreeNode n)
        {
            // If we have 0 or 1 children:
            if (n.Left == null || n.Right == null)
            {
                return n.Left ?? n.Right;
            }

            // If we have 2 children:
            // Use the maximum of the left subtree
            // or the minimum of the right subtree
            if (Static.random.Next(0, 2) == 0)
            {
                var heir = n.Left;
                while (heir.Right != null)
                {
                    heir = heir.Right;
                }

                return heir;
            }
            else
            {
                var heir = n.Right;
                while (heir.Left != null)
                {
                    heir = heir.Right;
                }

                return heir;
            }
        }

        public void Remove(TKey key)
        {
            if (IsEmpty)
            {
                return;
            }

            if (key.CompareTo(root.Key) == 0)
            {
                root = Heir(root);
                return;
            }

            var current = root;
            while (current != null)
            {
                switch (key.CompareTo(current.Key))
                {
                    case < 0 when current.Right == null:
                        // Key not found
                        return;
                    case < 0 when key.CompareTo(current.Right.Key) == 0:
                        current.Right = Heir(current.Right);
                        return;
                    case < 0:
                        current = current.Right;
                        break;
                    case > 0 when current.Left == null:
                        // Key not found
                        return;
                    case > 0 when key.CompareTo(current.Left.Key) == 0:
                        current.Left = Heir(current.Left);
                        return;
                    case > 0:
                        current = current.Left;
                        break;
                }
            }

            return;
        }

        public KeyValuePair<TKey, TValue>? Minimum()
        {
            var current = root;
            while (current?.Left != null)
            {
                current = current.Left;
            }

            if (current == null)
            {
                return null;
            }

            return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }

        public KeyValuePair<TKey, TValue>? Maximum()
        {
            var current = root;
            while (current?.Right != null)
            {
                current = current.Right;
            }

            if (current == null)
            {
                return null;
            }

            return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }
    }
}