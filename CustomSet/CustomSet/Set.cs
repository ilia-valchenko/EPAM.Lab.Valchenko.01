using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomSet
{
    /// <summary>
    /// This class represents a custom generic mathematics set with basic set's operations.
    /// </summary>
    /// <typeparam name="T">Type of elements in the set.</typeparam>
    public class Set<T> : IEquatable<Set<T>>, IEnumerable<T> where T : class, IEquatable<T>, IComparable<T>
    {
        #region Public fields and properties
        /// <summary>
        /// Count of elements in set.
        /// </summary>
        public int Count => collection.Count; 
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor which creates an empty set.
        /// </summary>
        public Set() : this(new List<T>()) { }

        /// <summary>
        /// This constructor creates a new set by the given set.
        /// </summary>
        /// <param name="set">Original set.</param>
        public Set(Set<T> set) : this(set as IEnumerable<T>) { }

        /// <summary>
        /// This constructor creates the set which consists of element of the given collection.
        /// </summary>
        /// <param name="collection">Collection of values.</param>
        public Set(IEnumerable<T> collection)
        {
            if(ReferenceEquals(collection, null))
                throw new ArgumentNullException(nameof(collection));

            //this.collection = new List<T>();
            this.collection = new BinarySearchTree<T>();

            foreach (T item in collection)
                Add(item);
        }
        #endregion

        #region Basic operations for set
        /// <summary>
        /// This method adds a new element to the set.
        /// </summary>
        /// <param name="insertItem">Element which must be insetred.</param>
        public void Add(T insertItem)
        {
            if (ReferenceEquals(insertItem, null))
                throw new ArgumentNullException(nameof(insertItem));

            if(collection.Count == 0)
                collection.Add(insertItem);
            else
                if (!collection.Contains(insertItem))
                collection.Add(insertItem);
        }

        /// <summary>
        /// This method deletes the given element from the set if it exists. If element doesn't exist then exception will be thrown.
        /// </summary>
        /// <param name="deletedItem">Element which must be deleted.</param>
        public void Delete(T deletedItem)
        {
            if (ReferenceEquals(deletedItem, null))
                throw new ArgumentNullException(nameof(deletedItem));

            if (!collection.Contains(deletedItem))
                throw new ArgumentException("Elemet which must be deleted not found.");

            collection.Remove(deletedItem);
        }

        #region Intersect operations
        /// <summary>
        /// This method returns the result of intersection of the current set and another one.
        /// </summary>
        /// <param name="otherSet">Another set which will be interseted to a current set.</param>
        public Set<T> Intersect(Set<T> otherSet)
        {
            if (ReferenceEquals(otherSet, null))
                throw new ArgumentNullException(nameof(otherSet));

            return new Set<T>((Union(otherSet)).Difference((Difference(otherSet)).Union(otherSet.Difference(this))));
        }

        /// <summary>
        /// This method returns the new set which represents the intersection of two given sets.
        /// </summary>
        /// <param name="firstSet">The first set.</param>
        /// <param name="secondSet">The second set.</param>
        /// <returns>Returns the new set which represents the intersection of two given sets.</returns>
        public static Set<T> Intersect(Set<T> firstSet, Set<T> secondSet)
        {
            if (ReferenceEquals(firstSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            // The Second set will be checked into instance intersect method.
            //if (ReferenceEquals(secondSet, null))
            //    throw new ArgumentNullException(nameof(secondSet));

            if (ReferenceEquals(firstSet, secondSet))
                return new Set<T>(firstSet);

            return firstSet.Intersect(secondSet);
        }

        /// <summary>
        /// This method returns the new set which represents the intersection of three given sets.
        /// </summary>
        /// <param name="firstSet">The first set.</param>
        /// <param name="secondSet">The second set.</param>
        /// <param name="thirdSet">The third set.</param>
        /// <returns>Returns the new set which represents the intersection of three given sets.</returns>
        public static Set<T> Intersect(Set<T> firstSet, Set<T> secondSet, Set<T> thirdSet)
        {
            if (ReferenceEquals(firstSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            //if (ReferenceEquals(secondSet, null))
            //    throw new ArgumentNullException(nameof(secondSet));

            //if (ReferenceEquals(thirdSet, null))
            //    throw new ArgumentNullException(nameof(thirdSet));

            return (firstSet.Intersect(secondSet)).Intersect(thirdSet);
        }

        /// <summary>
        /// This method returns the new set which represents the intersection of a few set.
        /// </summary>
        /// <param name="sets">Array of sets which must be intersected.</param>
        /// <returns>Returns the result of the intersection of sets.</returns>
        public static Set<T> Intersect(params Set<T>[] sets)
        {
            foreach (var set in sets)
                if (ReferenceEquals(set, null))
                    throw new ArgumentNullException("One of the given sets is null.");

            var result = sets[0];

            for (int i = 1; i < sets.Length; i++)
                result = result.Intersect(sets[i]);

            return result;
        } 
        #endregion

        #region Difference operations
        /// <summary>
        /// This method returns the difference of two sets.
        /// </summary>
        /// <param name="other">The second set for difference.</param>
        /// <returns>The result of difference of current set and given another.</returns>
        public Set<T> Difference(Set<T> other)
        {
            if (ReferenceEquals(other, null))
                throw new ArgumentNullException(nameof(other));

            return new Set<T>(collection.Where(item => !other.collection.Contains(item)).ToList());
        }

        /// <summary>
        /// This method returns the new set which represents the difference between firstSet and secondSet.
        /// </summary>
        /// <param name="firstSet">First set.</param>
        /// <param name="secondSet">Second set.</param>
        /// <returns>New set which represents the result of difference.</returns>
        public static Set<T> Difference(Set<T> firstSet, Set<T> secondSet)
        {
            if (ReferenceEquals(firstSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            if (ReferenceEquals(secondSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            return firstSet.Difference(secondSet);
        }

        /// <summary>
        /// This method returns the new set which represents the difference of a few set.
        /// </summary>
        /// <param name="sets">Array of sets which must be deducted.</param>
        /// <returns>Returns the result of the difference of sets.</returns>
        public static Set<T> Difference(params Set<T>[] sets)
        {
            foreach (var set in sets)
                if (ReferenceEquals(set, null))
                    throw new ArgumentNullException("One of the given sets is null.");

            var result = sets[0];

            for (int i = 1; i < sets.Length; i++)
                result = result.Difference(sets[i]);

            return result;
        }
        #endregion

        #region Union operations
        /// <summary>
        /// This method finds union of two sets.
        /// </summary>
        /// <param name="other">Another set.</param>
        /// <returns>Returns the new set which represents the result of union current and given another set.</returns>
        public Set<T> Union(Set<T> other)
        {
            if (ReferenceEquals(other, null))
                throw new ArgumentNullException(nameof(other));

            // fix it
            // CopyTo doesn't implement
            //var result  = new List<T>(collection);

            var result = new List<T>();
            foreach (var item in collection)
                result.Add(item);

            foreach (T item in other.collection)
                if (!result.Contains(item))
                    result.Add(item);

            return new Set<T>(result);
        }

        /// <summary>
        /// This static method finds union of two sets.
        /// </summary>
        /// <param name="firstSet">The first set.</param>
        /// <param name="secondSet">The second set.</param>
        /// <returns>Returns the new set which represents the result of union current and given another set.</returns>
        public static Set<T> Union(Set<T> firstSet, Set<T> secondSet)
        {
            if (ReferenceEquals(firstSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            if (ReferenceEquals(secondSet, null))
                throw new ArgumentNullException(nameof(secondSet));

            return firstSet.Union(secondSet);
        }

        /// <summary>
        /// This method returns the new set which represents the intersection of a few set.
        /// </summary>
        /// <param name="sets">Array of sets which must be intersected.</param>
        /// <returns>Returns the result of the intersection of sets.</returns>
        public static Set<T> Union(params Set<T>[] sets)
        {
            foreach (var set in sets)
                if (ReferenceEquals(set, null))
                    throw new ArgumentNullException("One of the given sets is null.");

            var result = sets[0];

            for (int i = 1; i < sets.Length; i++)
                result = result.Union(sets[i]);

            return result;
        }
        #endregion

        #region SymmetricDifference operations
        /// <summary>
        /// This method finds symmetric difference between two sets. 
        /// </summary>
        /// <param name="otherSet">Another set.</param>
        /// <returns>Returns a new set which represents the symmetric difference.</returns>
        public Set<T> SymmetricDifference(Set<T> otherSet)
        {
            if (ReferenceEquals(otherSet, null))
                throw new ArgumentNullException(nameof(otherSet));

            return new Set<T>((Union(otherSet)).Difference(Intersect(otherSet)));
        }

        public static Set<T> SymmetricDifference(Set<T> firstSet, Set<T> secondSet)
        {
            if (ReferenceEquals(firstSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            if (ReferenceEquals(secondSet, null))
                throw new ArgumentNullException(nameof(firstSet));

            return firstSet.SymmetricDifference(secondSet);
        } 
        #endregion
        #endregion

        #region IEnumerable inplementation
        /// <summary>
        /// This method returns enumerator for foreach loop.
        /// </summary>
        /// <returns>Returns enumerator.</returns>
        public IEnumerator<T> GetEnumerator() => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        /// <summary>
        /// This method gives a string representation of the set.
        /// </summary>
        /// <returns>Returns a string representation of the set.</returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var item in collection)
                result.Append(item + " ");

            return result.ToString();       
        }

        /// <summary>
        /// This method determines whether the current set equals to the another given set.
        /// </summary>
        /// <param name="other">An another set for determine equality.</param>
        /// <returns>Returns true if two sets contain the same elements.</returns>
        public bool Equals(Set<T> other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (collection.Count != other.collection.Count)
                return false;

            throw new NotImplementedException();
        }

        #region Private fields
        /// <summary>
        /// This List represents collection of elements which the set contains.
        /// </summary>
        private readonly BinarySearchTree<T> collection;
        #endregion
    }
}
