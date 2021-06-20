using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
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

        #region Helper methods

        /// <summary>
        /// Set the color of a node to the opposite of what it is now.
        /// </summary>
        private void ToggleColor(TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

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
        /// Swap the colors of two nodes.
        /// </summary>
        private void ExchangeColors(TreeNode n1, TreeNode n2)
        {
            if (n1 == null)
            {
                throw new ArgumentNullException(nameof(n1));
            }

            if (n2 == null)
            {
                throw new ArgumentNullException(nameof(n2));
            }

            var tmp = n1.Color;
            n1.Color = n2.Color;
            n2.Color = tmp;
        }

        /// <summary>
        /// Perform right-rotate on a node and return its right child, i.e. the pivot.
        /// </summary>
        private TreeNode RightRotate(TreeNode root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var pivot = root.Left;
            if (pivot == null)
            {
                throw new ArgumentException("Right rotate on node without left child");
            }

            ReplaceNode(root, pivot);
            root.Left = pivot.Right;
            if (root.Left != null)
            {
                root.Left.Parent = root;
            }
            
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
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            var pivot = root.Right;
            if (pivot == null)
            {
                throw new ArgumentException("Left rotate on node without right child");
            }

            ReplaceNode(root, pivot);
            root.Right = pivot.Left;
            if (root.Right != null)
            {
                root.Right.Parent = root;
            }
            
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
        /// Returns true if the node is black (including null-nodes).
        /// </summary>
        private bool IsBlack(TreeNode node)
        {
            return node == null || node.Color == Color.Black;
        }

        /// <summary>
        /// Returns true if the node is red.
        /// </summary>
        private bool IsRed(TreeNode node)
        {
            return !IsBlack(node);
        }

        /// <summary>
        /// Rotate a node's parent left if it is a right child, otherwise left.
        /// This also updates the parent of the rotation which may be the tree root.
        /// </summary>
        private void RotateParent(TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node.Parent == null)
            {
                throw new ArgumentException("RotateParent called on node without parent");
            }

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
            if (node?.Parent == null)
            {
                return null;
            }

            if (IsLeftChild(node))
            {
                return node.Parent.Right;
            }
            else
            {
                return node.Parent.Left;
            }
        }

        /// <summary>
        /// Return this node's uncle if it exists, else null.
        /// </summary>
        private TreeNode GetUncle(TreeNode node)
        {
            return GetSibling(node.Parent);
        }

        /// <summary>
        /// Get this node's distant nephew if it exists, else null.
        /// </summary>
        private TreeNode GetDistantNephew(TreeNode node)
        {
            var sibling = GetSibling(node);
            if (sibling == null)
            {
                return null;
            }

            return IsLeftChild(node) ? sibling.Right : sibling.Left;
        }

        /// <summary>
        /// Get this node's close nephew if it exists, else null.
        /// </summary>
        private TreeNode GetCloseNephew(TreeNode node)
        {
            var sibling = GetSibling(node);
            if (sibling == null)
            {
                return null;
            }

            return IsLeftChild(node) ? sibling.Left : sibling.Right;
        }

        /// <summary>
        /// Returns true if the node is the right child of a right child,
        /// or the left child of a left child.
        /// </summary>
        private bool IsOuterGrandchild(TreeNode node)
        {
            return IsLeftChild(node) == IsLeftChild(node.Parent);
        }

        /// <summary>
        /// Return this node's successor if it exists, else null.
        /// </summary>
        private TreeNode GetSuccessor(TreeNode node)
        {
            if (node.Right != null)
            {
                return MinimumNodeFromSubtree(node.Right);
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

        /// <summary>
        /// Return this node's predecessor if it exists, else null.
        /// </summary>
        private TreeNode GetPredecessor(TreeNode node)
        {
            if (node.Left != null)
            {
                return MaximumNodeFromSubtree(node.Left);
            }
            else
            {
                var current = node;
                while (!IsLeftChild(current))
                {
                    current = current.Parent;
                }

                return current.Parent;
            }
        }

        /// <summary>
        /// Return the maximum node from the subtree starting at a given node.
        /// </summary>
        private TreeNode MaximumNodeFromSubtree(TreeNode subtreeRoot)
        {
            if (subtreeRoot.Right == null)
            {
                return subtreeRoot;
            }

            return MaximumNodeFromSubtree(subtreeRoot.Right);
        }

        /// <summary>
        /// Return the minimmum node from the subtree starting at a given node.
        /// </summary>
        private TreeNode MinimumNodeFromSubtree(TreeNode subtreeRoot)
        {
            if (subtreeRoot.Left == null)
            {
                return subtreeRoot;
            }

            return MinimumNodeFromSubtree(subtreeRoot.Left);
        }

        /// <summary>
        /// Checks if a node has at least one red child.
        /// </summary>
        private bool HasRedChild(TreeNode node)
        {
            return IsRed(node?.Left) || IsRed(node?.Right);
        }

        /// <summary>
        /// Replace a node by putting a replacement in its position.
        /// </summary>
        /// <exception cref="ArgumentNullException">toReplace is null</exception>
        private void ReplaceNode(TreeNode toReplace, TreeNode replacement)
        {
            if (toReplace == null)
            {
                throw new ArgumentNullException(nameof(toReplace));
            }

            if (_root == toReplace)
            {
                _root = replacement;
                return;
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

        #endregion

        #region Insert

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

            var parent = node.Parent;
            var grandparent = parent.Parent;
            var uncle = GetUncle(node);

            // Case 2: the violating node is a direct child of the root
            // -> color the root black
            if (parent == _root)
            {
                _root.Color = Color.Black;
                VerifyRedBlackRules();
                return;
            }

            // If cases 1 and 2 don't apply, and the parent is black,
            // nothing needs to be done.
            if (IsBlack(parent))
            {
                VerifyRedBlackRules();
                return;
            }

            // Case 3: parent and uncle are red
            if (IsRed(uncle))
            {
                // re-color parent, grandparent and uncle
                ToggleColor(parent);
                ToggleColor(grandparent);
                ToggleColor(uncle);
                // now the grandparent might be violating
                FixAfterInsert(grandparent);
            }

            // Case 4: parent is red, uncle is black and node is an outer grandchild
            else if (IsOuterGrandchild(node))
            {
                // rotate grandparent
                RotateParent(parent);
                ToggleColor(parent);
                ToggleColor(grandparent);
            }

            // Case 5: parent is red, uncle is red and node is an inner grandchild
            else
            {
                // rotate parent
                RotateParent(node);
                FixAfterInsert(parent);
            }

            VerifyRedBlackRules();
        }

        #endregion

        #region Remove

        public void Remove(TKey key)
        {
            // Find key to delete
            var toDelete = FindNode(key);

            // If the key wasn't found, there's nothing to remove
            if (toDelete == null)
            {
                return;
            }

            DeleteNode(toDelete);
        }

        /// <summary>
        /// Delete a node from the tree and fix the red-black properties.
        /// </summary>
        private void DeleteNode(TreeNode toDelete)
        {
            // If the node has a right child, overwrite its key and value with 
            // its successor and delete the successor instead
            if (toDelete.Right != null)
            {
                var successor = MinimumNodeFromSubtree(toDelete.Right);
                toDelete.Key = successor.Key;
                toDelete.Value = successor.Value;
                DeleteNode(successor);
                return;
            }

            // Similarly, if the node has a left child, overwrite its key
            // and value with its predecessor and delete the predecessor instead
            if (toDelete.Left != null)
            {
                var predecessor = MaximumNodeFromSubtree(toDelete.Left);
                toDelete.Key = predecessor.Key;
                toDelete.Value = predecessor.Value;
                DeleteNode(predecessor);
                return;
            }

            // At this point, toDelete should have no children
            if (toDelete.Left != null || toDelete.Right != null)
            {
                throw new RedBlackTreeException("toDelete failed to find leaf node");
            }

            // If toDelete is red, just remove it.
            // Removing a red node doesn't lead to any violations.
            if (IsRed(toDelete))
            {
                ReplaceNode(toDelete, null);
                VerifyRedBlackRules();
                return;
            }

            FixAfterDelete(toDelete);
            ReplaceNode(toDelete, null);
            VerifyRedBlackRules();
        }

        private void FixAfterDelete(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            var sibling = GetSibling(node);
            var parent = node.Parent;
            var closeNephew = GetCloseNephew(node);
            var distantNephew = GetDistantNephew(node);

            // Case 1: parent, sibling and both nephews are black
            if (IsBlack(parent) && IsBlack(sibling) && IsBlack(closeNephew) && IsBlack(distantNephew))
            {
                if (sibling != null)
                {
                    ToggleColor(sibling);
                }

                FixAfterDelete(parent);
                return;
            }

            // Case 2: the current node is the root -> nothing to do
            if (node == _root)
            {
                return;
            }

            // Case 3: the sibling is red -> rotate parent
            if (IsRed(sibling))
            {
                RotateParent(sibling);
                ToggleColor(parent);
                ToggleColor(sibling);
                // fall through to another case
            }

            // Case 4: sibling and nephews are black, but parent is red
            // -> move the red down to the sibling
            if (IsBlack(sibling) && IsBlack(closeNephew) && IsBlack(distantNephew) && IsRed(parent))
            {
                ToggleColor(sibling);
                ToggleColor(parent);
                return;
            }

            // Case 5: sibling and distant nephew are black, but close nephew is red
            // -> rotate sibling around close nephew
            if (IsBlack(sibling) && IsBlack(distantNephew) && IsRed(closeNephew))
            {
                RotateParent(closeNephew);
                ExchangeColors(sibling, closeNephew);
                // fall through to case 6
            }

            // Case 6: sibling is black, distant nephew is red
            if (IsBlack(sibling) && IsRed(distantNephew))
            {
                RotateParent(sibling);
                ExchangeColors(parent, sibling);
                ToggleColor(distantNephew);
            }
        }

        #endregion

        #region Search + Traverse

        public TValue Search(TKey key)
        {
            return FindNode(key)?.Value;
        }

        public KeyValuePair<TKey, TValue>? Minimum()
        {
            var node = MinimumNodeFromSubtree(_root);
            return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }

        public KeyValuePair<TKey, TValue>? Maximum()
        {
            var node = MaximumNodeFromSubtree(_root);
            return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }

        #endregion

        #region Verify rules

#if DEBUG
        
        /// <summary>
        /// Check that all red-black rules are met. This method is only for verification
        /// purposes when debugging. It's turned off in release builds.
        /// </summary>
        private void VerifyRedBlackRules()
        {
            if (IsRed(_root))
            {
                throw new RedBlackTreeException("red-black rule violated: root was red");
            }

            int? blackHeight = null;
            VerifyRedBlackRules(_root, ref blackHeight);
        }

        private void VerifyRedBlackRules(TreeNode node, ref int? blackHeight)
        {
            if (node == null)
            {
                return;
            }

            if (node.Left == null || node.Right == null)
            {
                int blackHeightHere = MeasureBlackHeight(node);
                blackHeight ??= blackHeightHere;
                if (blackHeight != blackHeightHere)
                {
                    throw new RedBlackTreeException(
                        $"red-black rule violated: black-height at node {node.Key} " +
                        $"was {blackHeightHere}, was {blackHeight} somewhere else");
                }
            }

            if (node.Left != null && node.Left.Parent != node)
            {
                throw new RedBlackTreeException(
                    $"tree rule violated: at node {node.Key}: node.Left.Parent != node");
            }

            if (node.Right != null && node.Right.Parent != node)
            {
                throw new RedBlackTreeException(
                    $"tree rule violated: at node {node.Key}: node.Right.Parent != node");
            }

            if (IsRed(node.Left) && IsRed(node))
            {
                throw new RedBlackTreeException(
                    $"red-black rule violated: node {node.Key} was red " +
                    $"and its left child {node.Left!.Key} was also red");
            }

            if (IsRed(node.Right) && IsRed(node))
            {
                throw new RedBlackTreeException(
                    $"red-black rule violated: node {node.Key} was red " +
                    $"and its right child {node.Right!.Key} was also red");
            }

            VerifyRedBlackRules(node.Left, ref blackHeight);
            VerifyRedBlackRules(node.Right, ref blackHeight);
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

            if (IsBlack(node))
            {
                return 1 + MeasureBlackHeight(node.Parent);
            }

            return MeasureBlackHeight(node.Parent);
        }

#else

        private void VerifyRedBlackRules() 
        {
            // Verification is turned off in release builds for improved performance
        }

#endif

        #endregion

        [Serializable]
        public class RedBlackTreeException : Exception
        {
            public RedBlackTreeException()
            {
            }

            public RedBlackTreeException(string message) : base(message)
            {
            }

            public RedBlackTreeException(string message, Exception inner) : base(message, inner)
            {
            }

            protected RedBlackTreeException(
                SerializationInfo info,
                StreamingContext context) : base(info, context)
            {
            }
        }
    }
}