using System;
using System.Collections.Generic;
using System.Diagnostics;
using Algodat.SortAlgorithms;

namespace Algodat.Trees
{
    public class RedBlackTree<TKey, TValue> : ITree<TKey, TValue> where TKey : IComparable<TKey> where TValue : class
    {
        private enum Color
        {
            Black,
            Red
        }

        private enum Direction
        {
            Left,
            Right
        }

        private class TreeNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Color Color { get; set; }

            public TreeNode Parent { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }

            public TreeNode(TKey key, TValue value, Color color)
            {
                Key = key;
                Value = value;
                Color = color;
            }
        }

        private TreeNode _root;
        private bool IsEmpty => _root == null;

        /// <summary>
        /// Set the color of a node to the opposite of what it is now.
        /// </summary>
        private void ToggleColor(TreeNode node)
        {
            if (node.Color == Color.Black)
            {
                node.Color = Color.Red;
            }
            else
            {
                node.Color = Color.Black;
            }
        }

        /// <summary>
        /// Perform right-rotate on a node and return its right child, i.e. the pivot.
        /// </summary>
        private TreeNode RightRotate(TreeNode root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            var pivot = root.Left;
            if (pivot == null) throw new ArgumentException("Right rotate on node without left child");

            root.Left = pivot.Right;
            pivot.Right = root;
            pivot.Parent = root.Parent;
            root.Parent = pivot;
            return pivot;
        }

        /// <summary>
        /// Perform left-rotate on a node and return its left child, i.e. the pivot.
        /// </summary>
        private TreeNode LeftRotate(TreeNode root)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            var pivot = root.Right;
            if (pivot == null) throw new ArgumentException("Left rotate on node without right child");

            root.Right = pivot.Left;
            pivot.Left = root;
            pivot.Parent = root.Parent;
            root.Parent = pivot;
            return pivot;
        }

        /// <summary>
        /// Returns true if the given node is a left child of its parents.
        /// Throws if there is no parent!
        /// </summary>
        private bool IsLeftChild(TreeNode node)
        {
            return node == node.Parent.Left;
        }

        /// <summary>
        /// Rotate a node's parent left if it is a right child, otherwise left.
        /// This also updates the parent of the rotation which may be the tree root.
        /// </summary>
        private void RotateParent(TreeNode node)
        {
            if (node.Parent == _root)
            {
                _root = IsLeftChild(node)
                    ? RightRotate(node.Parent)
                    : LeftRotate(node.Parent);
            }
            else
            {
                node.Parent.Parent = IsLeftChild(node)
                    ? RightRotate(node.Parent)
                    : LeftRotate(node.Parent);
            }
        }

        /// <summary>
        /// Returns this node's sibling if it exists, else null.
        /// </summary>
        private TreeNode GetSibling(TreeNode node)
        {
            return IsLeftChild(node)
                ? node.Parent.Right
                : node.Parent.Left;
        }

        /// <summary>
        /// Return this node's uncle if it exists, else null.
        /// </summary>
        private TreeNode GetUncle(TreeNode node)
        {
            return GetSibling(node.Parent);
        }

        /// <summary>
        /// Returns true if the node is the right child of a right child,
        /// or the left child of a left child.
        /// </summary>
        private bool IsOuterGrandchild(TreeNode node)
        {
            return IsLeftChild(node) == IsLeftChild(node.Parent);
        }

        public void Insert(TKey key, TValue value)
        {
            var newNode = new TreeNode(key, value, Color.Red);

            // Find insert position by normal binary tree rules
            TreeNode parent = null;
            Direction dir = Direction.Left;
            var current = _root;

            while (current != null)
            {
                switch (key.CompareTo(current.Key))
                {
                    case < 0:
                        dir = Direction.Left;
                        parent = current;
                        current = current.Left;
                        break;
                    case > 0:
                        dir = Direction.Right;
                        parent = current;
                        current = current.Right;
                        break;
                    case 0:
                        // Key already exists, we just override the value
                        current.Value = value;
                        return; // Insertion complete
                }
            }

            // Insert child
            if (parent == null)
            {
                _root = newNode;
            }
            else
            {
                if (dir == Direction.Left)
                {
                    parent.Left = newNode;
                }
                else
                {
                    parent.Right = newNode;
                }

                newNode.Parent = parent;
            }

            // Restore red-black properties
            FixAfterInsert(newNode);
        }

        private void FixAfterInsert(TreeNode node)
        {
            // Case 1: the violating node is the root -> color it black
            if (node == _root)
            {
                node.Color = Color.Black;
                VerifyRedBlackRules();
                return;
            }

            // Case 2: the violating node is a direct child of the root
            // -> color the root black
            if (node.Parent == _root)
            {
                _root.Color = Color.Black;
                VerifyRedBlackRules();
                return;
            }

            var parent = node.Parent;
            var grandparent = parent.Parent;
            var uncle = GetUncle(node);

            // Case 3: the uncle is red
            if (uncle?.Color == Color.Red)
            {
                // re-color parent, grandparent and uncle
                ToggleColor(parent);
                ToggleColor(grandparent);
                ToggleColor(uncle);
                // now the grandparent might be violating
                FixAfterInsert(grandparent);
            }

            // Case 4: the uncle is black and node is an outer grandchild
            else if (IsOuterGrandchild(node))
            {
                // rotate grandparent
                RotateParent(parent);
                ToggleColor(parent);
                ToggleColor(grandparent);
            }

            // Case 5: the uncle is red and node is an inner grandchild
            else
            {
                // rotate parent
                RotateParent(node);
                FixAfterInsert(parent);
            }

            VerifyRedBlackRules();
        }

        /// <summary>
        /// Replace a node by putting a replacement in its position.
        /// Also changes the replacement's children to those of the deleted node.
        /// </summary>
        /// <exception cref="ArgumentNullException">toReplace is null</exception>
        private void ReplaceNode(TreeNode toReplace, TreeNode replacement)
        {
            if (toReplace == null) throw new ArgumentNullException(nameof(toReplace));
            if (replacement != null)
            {
                replacement.Left = toReplace.Left;
                replacement.Right = toReplace.Right;
            }
            
            if (_root == toReplace)
            {
                _root = replacement;
            }
            if (IsLeftChild(toReplace))
            {
                toReplace.Parent.Left = replacement;
            }
            else
            {
                toReplace.Parent.Right = replacement;
            }
        }

        /// <summary>
        /// Return this node's successor if it exists, else null.
        /// </summary>
        private TreeNode GetSuccessor(TreeNode node)
        {
            if (node.Right != null)
            {
                var current = node.Right;
                while (current.Left != null)
                {
                    current = current.Left;
                }

                return current;
            }
            else
            {
                var current = node;
                while (IsLeftChild(current))
                {
                    current = current.Parent;
                }

                return current.Parent;
            }
        }

        public void Remove(TKey key)
        {
            // Find key to delete
            var toDelete = FindNode(key);
            if (toDelete == null)
            {
                return;
            }

            if (toDelete == _root)
            {
                _root = null;
                return;
            }

            TreeNode x = null;
            TreeNode replacement = null;
            
            // If both children are null, replace the node with null
            // If one child is null, replace with the non-null child
            if (toDelete.Left == null || toDelete.Right == null)
            {
                x = replacement = toDelete.Left ?? toDelete.Right;
            }

            // If both children are non-null, find the replacement and set
            // x to the replacements right child
            else
            {
                replacement = GetSuccessor(toDelete);
                x = replacement?.Right;
            }
            
            ReplaceNode(toDelete, replacement);

            if (toDelete.Color == Color.Red)
            {
                // Case 1: deleted node is red, replacement is null or red
                if (x == null || x.Color == Color.Red)
                {
                    return;
                }
                // Case 2: deleted node is red, replacement is also red
                else
                {
                    x.Color = Color.Red;
                    // TODO proceed to appropriate case
                }
            }
            else
            {
                // Case 3: deleted node is black, replacement is null or black
                if (x == null || x.Color == Color.Black)
                {
                    // TODO proceed to appropriate case
                }
                // Case 4: deleted node is black, replacement is red
                else
                {
                    x.Color = Color.Black;
                    return;
                }
            }
        }

        #region Verify rules

        /// <summary>
        /// Check that all red-black rules are met. This method is only for verification
        /// purposes when debugging. It's turned off in release builds.
        /// </summary>
        private void VerifyRedBlackRules()
        {
#if DEBUG
            if (_root.Color == Color.Red)
            {
                throw new Exception("red-black rule violated: root was red");
            }

            VerifyRedBlackRules(_root, null);
#endif
        }

        private void VerifyRedBlackRules(TreeNode node, int? blackHeight)
        {
            if (node == null)
            {
                return;
            }

            if (node.Left == null)
            {
                int blackHeightHere = MeasureBlackHeight(node);
                blackHeight ??= blackHeightHere;
                if (blackHeight != blackHeightHere)
                {
                    throw new Exception(
                        $"red-black rule violated: black-height at node {node.Key} " +
                        $"was {blackHeightHere}, was {blackHeight} somewhere else");
                }
            }
            else if (node.Left.Color == Color.Red && node.Color == Color.Red)
            {
                throw new Exception(
                    $"red-black rule violated: node {node.Key} was red " +
                    $"and its left child {node.Left.Key} was also red");
            }

            VerifyRedBlackRules(node.Left, blackHeight);

            if (node.Right?.Color == Color.Red && node.Color == Color.Red)
            {
                throw new Exception(
                    $"red-black rule violated: node {node.Key} was red " +
                    $"and its right child {node.Right.Key} was also red");
            }

            VerifyRedBlackRules(node.Right, blackHeight);
        }

        /// <summary>
        /// Return the number of black nodes between this node and the root (inclusive).
        /// </summary>
        private int MeasureBlackHeight(TreeNode node)
        {
            // the root is always black, this was checked earlier
            if (node == _root)
            {
                return 1;
            }

            if (node.Color == Color.Black)
            {
                return 1 + MeasureBlackHeight(node.Parent);
            }

            return MeasureBlackHeight(node.Parent);
        }

        #endregion

        private TreeNode FindNode(TKey key)
        {
            var current = _root;
            while (current != null)
            {
                switch (key.CompareTo(current.Key))
                {
                    case < 0:
                        current = current.Left;
                        break;
                    case 0:
                        return current;
                    case > 0:
                        current = current.Right;
                        break;
                }
            }

            return null;
        }
        
        public TValue Search(TKey key)
        {
            return FindNode(key)?.Value;
        }

        public KeyValuePair<TKey, TValue>? Minimum()
        {
            var current = _root;
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
            var current = _root;
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