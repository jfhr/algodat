using System;
using System.Collections.Generic;

namespace Algodat.Trees
{
    public class BinarySearchTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey> where TValue : class
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

            public TValue Search(TKey key)
            {
                return key.CompareTo(Key) switch
                {
                    < 0 => Left?.Search(key),
                    > 0 => Right?.Search(key),
                    0 => Value
                };
            }

            public void Insert(TKey key, TValue value)
            {
                switch (key.CompareTo(Key))
                {
                    case 0:
                        Value = value;
                        break;
                    case < 0 when Left == null:
                        Left = new TreeNode(key, value);
                        break;
                    case < 0:
                        Left.Insert(key, value);
                        break;
                    case > 0 when Right == null:
                        Right = new TreeNode(key, value);
                        break;
                    case > 0:
                        Right.Insert(key, value);
                        break;
                }
            }

            /// <summary>
            /// Remove the given key from any of the children of this tree, if it is found there.
            /// Assumes that the key is not equal to the key of this node itself.
            /// </summary>
            public void RemoveFromChildren(TKey key)
            {
                if (key.CompareTo(Key) < 0)
                {
                    if (Right == null)
                    {
                        // Key not found
                        return;
                    }

                    if (key.CompareTo(Right.Key) == 0)
                    {
                        Right = Right.Heir();
                    }
                    else
                    {
                        Right.RemoveFromChildren(key);
                    }
                }
                else
                {
                    if (Left == null)
                    {
                        // Key not found
                        return;
                    }

                    if (key.CompareTo(Left.Key) == 0)
                    {
                        Left = Left.Heir();
                    }
                    else
                    {
                        Left.RemoveFromChildren(key);
                    }
                }
            }

            /// <summary>
            /// Return the node that replaces this node when it is removed.
            /// May return null.
            /// </summary>
            public TreeNode Heir()
            {
                // If we have 0 or 1 children:
                if (Left == null || Right == null)
                {
                    return Left ?? Right;
                }

                // If we have 2 children:
                // Here we can use either Left.Maximum() or Right.Minimum();
                // We could use the same every time, but that increases the chance of
                // getting an unbalanced tree, so instead we choose one at random.
                return Shared.Random.Next(0, 2) == 0
                    ? Left.Maximum()
                    : Right.Minimum();
            }

            public TreeNode Minimum() => Left?.Minimum() ?? this;
            public TreeNode Maximum() => Right?.Maximum() ?? this;
        }

        private TreeNode _root;

        private bool IsEmpty => _root == null;

        public TValue Search(TKey key)
        {
            if (IsEmpty)
            {
                return null;
            }

            return _root.Search(key);
        }

        public void Insert(TKey key, TValue value)
        {
            if (IsEmpty)
            {
                _root = new TreeNode(key, value);
            }
            else
            {
                _root.Insert(key, value);
            }
        }

        public void Remove(TKey key)
        {
            if (IsEmpty)
            {
                return;
            }

            if (key.CompareTo(_root.Key) == 0)
            {
                _root = _root.Heir();
            }
            else
            {
                _root.RemoveFromChildren(key);
            }
        }

        public KeyValuePair<TKey, TValue> Minimum()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            var node = _root.Minimum();
            return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }

        public KeyValuePair<TKey, TValue> Maximum()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException();
            }

            var node = _root.Maximum();
            return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }
    }
}