/*
 * File: BaseSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the implementation of the abstract BaseSet class, which provides 
 * a foundation for structured set implementations. It defines common operations and properties 
 * for handling sets, including adding, removing, checking for elements, and merging sets. 
 * The class uses generics to support sets with any comparable type of elements.
 * 
 * Key Features:
 * - Provides an abstract class with methods to manage sets and nested subsets.
 * - Supports operations like adding, removing, and checking for elements and subsets.
 * - Includes functionality for merging sets, evaluating subset relationships, and cardinality.
 * - Allows for flexible set operations with a generic type parameter, enabling usage with different element types.
 */

using SetsLibrary.Interfaces;

namespace SetsLibrary.Models
{
    /// <summary>
    /// An abstract base class for structured sets, providing a foundation for specific set implementations.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set, which must be comparable.</typeparam>
    public abstract class BaseSet<T> : IStructuredSet<T>
        where T : IComparable
    {
        /// <summary>
        /// Gets the set tree element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>The <see cref="ISetTree{T}"/> at the specified index.</returns>
        /// <exception cref="NotImplementedException">This method is not implemented.</exception>
        public ISetTree<T> this[int index] => throw new NotImplementedException();

        /// <summary>
        /// Gets the evaluated string representation of the set, with duplicates removed.
        /// </summary>
        public string ElementString => throw new NotImplementedException();

        /// <summary>
        /// Gets the original string representation of the set.
        /// </summary>
        public string OriginalString => throw new NotImplementedException();

        /// <summary>
        /// Gets the cardinality (number of elements) of the evaluated set.
        /// </summary>
        public int Cardinality => throw new NotImplementedException();

        /// <summary>
        /// Gets the current settings of the set extractor.
        /// </summary>
        public SetExtractionSettings<T> Settings => throw new NotImplementedException();

        /// <summary>
        /// Adds a new element to the set. If the element already exists, it will not be added.
        /// </summary>
        /// <param name="Element">The element to be added.</param>
        public void AddElement(T Element)
        {
            throw new NotImplementedException();
        }//AddElement

        /// <summary>
        /// Adds a new tree as an element in the current set. If the tree already exists, it will not be added.
        /// </summary>
        /// <param name="tree">The tree to be added.</param>
        public void AddElement(ISetTree<T> tree)
        {
            throw new NotImplementedException();
        }//AddElement

        /// <summary>
        /// Adds a new subset to the current set from a string representation of the subset.
        /// </summary>
        /// <param name="subset">The string representation of the subset.</param>
        public void AddSubsetAsString(string subset)
        {
            throw new NotImplementedException();
        }//AddSubsetAsString

        /// <summary>
        /// Clears all elements in the current set.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }//Clear


        /// <summary>
        /// Checks if the specified tree exists in the set.
        /// </summary>
        /// <param name="tree">The tree to check for.</param>
        /// <returns>True if the tree exists; otherwise, false.</returns>
        public bool Contains(ISetTree<T> tree)
        {
            throw new NotImplementedException();
        }//Contains

        /// <summary>
        /// Checks if the current set is an element of the specified set.
        /// </summary>
        /// <param name="setB">The set to check against.</param>
        /// <returns>True if the current set is an element of setB; otherwise, false.</returns>
        public bool IsElementOf(IStructuredSet<T> setB)
        {
            throw new NotImplementedException();
        }//IsElementOf

        /// <summary>
        /// Determines if the current set is a subset of the specified set.
        /// </summary>
        /// <param name="setB">The set to check against.</param>
        /// <param name="type">The type of set relationship.</param>
        /// <returns>True if the current set is a subset; otherwise, false.</returns>
        public bool IsSubSetOf(IStructuredSet<T> setB, out SetResultType type)
        {
            throw new NotImplementedException();
        }//IsSubSetOf

        /// <summary>
        /// Removes the specified tree from the current set.
        /// </summary>
        /// <param name="tree">The tree to remove.</param>
        /// <returns>True if the tree was found and removed; otherwise, false.</returns>
        public bool RemoveElement(ISetTree<T> tree)
        {
            throw new NotImplementedException();
        }//RemoveElement

        /// <summary>
        /// Removes the specified element from the current set.
        /// </summary>
        /// <param name="Element">The element to remove.</param>
        /// <returns>True if the element was found and removed; otherwise, false.</returns>
        public bool RemoveElement(T Element)
        {
            throw new NotImplementedException();
        }//RemoveElement

        /// <summary>
        /// Removes all elements of the specified set from the current set and returns the resulting set.
        /// </summary>
        /// <param name="setB">The set to remove elements from.</param>
        /// <returns>The resulting set after removal.</returns>
        public IStructuredSet<T> Without(IStructuredSet<T> setB)
        {
            throw new NotImplementedException();
        }//Without

        #region Abstract methods
        /// <summary>
        /// Merges the current set with another set and returns the resulting set.
        /// </summary>
        /// <param name="set">The set to merge with.</param>
        /// <returns>The merged set.</returns>
        public abstract IStructuredSet<T> MergeWith(IStructuredSet<T> set);

        /// <summary>
        /// Checks if the specified element exists in the set.
        /// </summary>
        /// <param name="Element">The element to check for.</param>
        /// <returns>True if the element exists; otherwise, false.</returns>
        public abstract bool Contains(T Element);
        #endregion ABSTRACT METHODS
    }//class
}//namespace