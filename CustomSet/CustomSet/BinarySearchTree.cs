using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomSet
{
    /// <summary>
    /// This class represents a simple binary search tree. It's a rooted binary tree, whose internal nodes 
    /// each store a key and each have two distinguished sub-trees, commonly denoted left and right.
    /// </summary>
    /// <typeparam name="T">Type of tree's node.</typeparam>
    public class BinarySearchTree<T> : ICollection<T>, IEnumerable<T> where T : IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// Number of elements of tree.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BinarySearchTree()
        {
            head = null;
        }

        /// <summary>
        /// This constructor use the given collection for creating a new tree.
        /// </summary>
        /// <param name="collection">The collection of the values.</param>
        public BinarySearchTree(IEnumerable<T> collection)
        {
            if (ReferenceEquals(collection, null))
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection)
                Add(item);
        }

        /// <summary>
        /// This method adds a new node which contains the given value to the tree.
        /// </summary>
        /// <param name="value">The value of the new node.</param>
        public void Add(T value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException(nameof(value));

            if (ReferenceEquals(head, null))
            {
                head = new Node(value);
                count++;
                return;
            }
                  
            AddTo(head, value);
            count++;
        }

        private void AddTo(Node node, T value)
        {
            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new Node(value);
                }
                else
                {
                    AddTo(node.Left, value);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new Node(value);
                }
                else
                {
                    AddTo(node.Right, value);
                }
            }
        }

        /// <summary>
        /// This method deletes node which contains the given value from the tree.
        /// </summary>
        /// <param name="item">The value of the node which must be deleted.</param>
        /// <returns>Returns true, if delete operation is successful.</returns>
        public bool Remove(T item) => Remove(head, item);

        private Node FindMin(Node node)
        {
            if (ReferenceEquals(node, null))
                return null;

            if (ReferenceEquals(node.Left, null))
                return node;

            return FindMin(node.Left);
        }

        private bool Remove(Node node, T item)
        {
            if (ReferenceEquals(node, null))
                return false;

            if (item.CompareTo(node.Value) < 0)
            {
                Remove(node.Left, item);
            }
            else
            {
                if (item.CompareTo(node.Value) > 0)
                {
                    Remove(node.Right, item);
                }
                else
                {
                    // Current node is sought-for node.
                    if (node.Left != null && node.Right != null)
                    {
                        // Replace current node from the minimal element from the right sub-tree.
                        node.Value = FindMin(node.Right).Value;
                        // Delete element from the right sub-tree by using recursion.
                        Remove(node.Right, node.Value);
                    }
                    else
                    {
                        //node = node.Left != null ? node.Left : node.Right;

                        //if (node.Left != null)
                        //    node = node.Left;
                        //else
                        //    node = node.Right;

                        if (node.Left == null && node.Right == null)
                        {
                            node = null;
                        }
                        else
                        {
                            if (node.Left != null)
                                node = node.Left;
                            else
                                node = node.Right;
                        }    
                    }
                }  
            }

            return true;
        }

        /// <summary>
        /// This method finds the node which contains the given value.
        /// </summary>
        /// <param name="value">The value of the sought-for node.</param>
        /// <returns>Returns the node which contains the given value.</returns>
        public Node FindByValue(T value) => Find(head, value);

        private Node Find(Node node, T value)
        {
            if (ReferenceEquals(value, null))
                throw new ArgumentNullException(nameof(value));

            if (ReferenceEquals(head, null))
                throw new InvalidOperationException("Tree is empty!");

            if (ReferenceEquals(node, null))
                return null;
                // not found
                //throw new ArgumentNullException(nameof(node));

            if (node.Value.CompareTo(value) == 0)
                return node;

            if (node.Value.CompareTo(value) > 0)
                return Find(node.Left, value);

            return Find(node.Right, value);
        }

        /// <summary>
        /// In this traversal method, the root node is visited first, then left subtree and finally right sub-tree.
        /// </summary>
        /// <returns>Returns elements of the tree in the preorder traversal.</returns>
        public IEnumerator<T> Preorder()
        {
            if (ReferenceEquals(head, null))
                throw new InvalidOperationException("Tree is empty!");

            var roots = new Stack<Node>();

            if (!ReferenceEquals(head.Left, null))
                roots.Push(head.Left);

            if (!ReferenceEquals(head.Right, null))
                roots.Push(head.Right);

            while (roots.Any())
            {
                var root = roots.Pop();
                yield return root.Value;

                if (!ReferenceEquals(root.Left, null))
                    roots.Push(root.Left);

                if (!ReferenceEquals(root.Right, null))
                    roots.Push(root.Right);
            }
        }

        /// <summary>
        /// In this traversal method, the left left-subtree is visited first, then root and then the 
        /// right sub-tree. We should always remember that every node may represent a subtree itself.
        /// </summary>
        /// <returns>Returns elements of the tree in the inorder traversal.</returns>
        public IEnumerator<T> Inorder()
        {
            if (ReferenceEquals(head, null))
                throw new InvalidOperationException("Tree is empty!");

            var roots = new Stack<Node>();
            var current = head;
            bool isDone = false;

            while (!isDone)
            {
                if (!ReferenceEquals(current, null))
                {
                    roots.Push(current);
                    current = current.Left;
                }
                else
                {
                    if (!roots.Any())
                    {
                        isDone = true;
                    }
                    else
                    {
                        current = roots.Pop();
                        yield return current.Value;
                        current = current.Right;
                    }
                }
            }
        }


        // recursion
        // fix it

        /// <summary>
        /// In this traversal method, the root node is visited last, hence the name. First we traverse left subtree, 
        /// then right subtree and finally root.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Returns elements of the tree in the postorder traversal.</returns>
        public IEnumerator<T> Postorder(Node node)
        {
            if (ReferenceEquals(node, null))
                throw new ArgumentNullException(nameof(node));

            if (!ReferenceEquals(node.Left, null))
                Postorder(node.Left);

            if (!ReferenceEquals(node.Right, null))
                Postorder(node.Right);

            yield return node.Value;
        }

        /// <summary>
        /// This method returns true if the BST is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// This method clears the current tree.
        /// </summary>
        public void Clear()
        {
            head = null;
            count = 0;
        }

        /// <summary>
        /// This method defines does the given element is contains in the tree.
        /// </summary>
        /// <param name="item">The value of the sought-for node.</param>
        /// <returns>Returns true if the tree contains the given element.</returns>
        public bool Contains(T item) => FindByValue(item) != null ? true : false;

        /// <summary>
        /// This method copies tree into an array.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The start index.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// This method returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through a collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            //return Preorder();
            return Inorder();
            //return Postorder(head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This inner class represents a node of a binary search tree.
        /// </summary>
        public class Node
        {
            /// <summary>
            /// A left child of a node.
            /// </summary>
            public Node Left { get; set; }
            /// <summary>
            /// A right child of a node.
            /// </summary>
            public Node Right { get; set; }
            /// <summary>
            /// A value which node contains.
            /// </summary>
            public T Value
            {
                get
                {
                    return value;
                }
                set
                {
                    if (ReferenceEquals(value, null))
                        throw new ArgumentException("Value of tree's node can't be null.");

                    this.value = value;
                }
            }
            /// <summary>
            /// Default constructor.
            /// </summary>
            public Node() : this(default(T)) { }
            /// <summary>
            /// This constructor creates a new node with the given value.
            /// </summary>
            /// <param name="value"></param>
            public Node(T value)
            {
                Value = value;
            }

            /*public int CompareTo(Node other) => value.CompareTo(other.value);

            public bool Equals(Node other)
            {
                if(ReferenceEquals(other, null))
                    throw new ArgumentNullException(nameof(other));

                if (this.Value.Equals(other.Value))
                    return true;

                return false;
            }*/

            public override string ToString()
            {
                if (ReferenceEquals(Left, null))
                {
                    if (ReferenceEquals(Right, null))
                        return $"Value: {value}, left: empty, right: empty.";
                    return $"Value: {value}, left: empty, right: {Right.Value}";
                }

                if (ReferenceEquals(Right, null))
                    return $"Value: {value}, left: {Left.value}, right: empty.";

                return $"Value: {value}, left: {Left.value}, right: {Right.value}.";
            }

            private T value;
        }

        private Node head;
        private int count = 0;
    }
}
