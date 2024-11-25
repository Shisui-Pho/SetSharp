/*
 * File: IStructuredSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the definition of the IStructuredSet interface, which outlines 
 * methods and properties for managing structured sets with elements and nested subsets.
 * It allows for operations such as adding, removing, and checking for elements or subsets, 
 * as well as handling cardinality and set relationships (e.g., subsets and element inclusion).
 * 
 * Key Features:
 * - Defines methods for managing sets, including adding and removing elements or subsets.
 * - Provides functionality for evaluating set relationships, such as subset checks.
 * - Supports nested subsets and element retrieval through indexers.
 * - Includes cardinality and extraction settings for structured sets.
 */

using SetsLibrary.Models;
namespace SetsLibrary.Interfaces
{
    public interface IStructuredSet<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Get the evaluated string representation of the set.
        /// Note : Duplicates will be removed
        /// </summary>
        string ElementString { get; }
        /// <summary>
        /// Gets the original set string
        /// </summary>
        string OriginalString { get; }
        /// <summary>
        /// Gets the Cardinality of the evaluated set.
        /// </summary>
        int Cardinality { get; }
        /// <summary>
        /// An indexer that returns a "Set" of an element inside the SetTree.
        ///     If the the element is in the root, it will be returned in a "Set" format.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A set of ISetTree<typeparamref name="T"/></returns>
        ISetTree<T> this[int index] { get; }
        /// <summary>
        /// Gets the current settings of the set extractor.
        /// </summary>
        SetExtractionConfiguration<T> Settings { get; }
        /// <summary>
        /// Adds a new element in the current set. If the element already exists it will not be added.
        /// </summary>
        /// <param name="Element">Element to be added</param>
        void AddElement(T Element);
        /// <summary>
        /// Adds a new tree as an element in the current set. If the element tree already exists it will not be added.
        /// This element could be a set or just an single element and will be on the first nesting level.
        /// </summary>
        /// <param name="tree"></param>
        void AddElement(ISetTree<T> tree);
        /// <summary>
        /// Adds a new subset in the current string by exctracting the tree.
        /// </summary>
        /// <param name="subset">A string representation of the subset.</param>
        void AddSubsetAsString(string subset);
        /// <summary>
        /// Adds a new tree as an element in the current set. If the tree already exists it will not be added.
        /// The tree will be on the first nesting level of the current set.
        /// </summary>
        /// <param name="tree">The tree to be removed</param>
        bool RemoveElement(ISetTree<T> tree);
        /// <summary>
        /// Adds a set as a subset of the current set. This set will be an element on the first nesting level of the current set.
        /// </summary>
        /// <param name="set"></param>
        IStructuredSet<T> MergeWith(IStructuredSet<T> set);
        /// <summary>
        /// Removes all elements of SetB that are in setA and return the resulting set.
        /// </summary>
        /// <param name="setB"></param>
        /// <returns>A new setA without elements in setB.</returns>
        IStructuredSet<T> Without(IStructuredSet<T> setB);
        /// <summary>
        /// Removes an element in the current set. This element could be a set or just an single element.
        /// This element must be on the first nesting level.
        /// </summary>
        /// <param name="Element">The element tree to be removed</param>
        /// <returns>Returns a bool indicating if the element was found and removed or not</returns>
        bool RemoveElement(T Element);
        /// <summary>
        /// Removes an element in the current set. This element could be a set or just an single element.
        /// This element must be on the first nesting level.
        /// </summary>
        /// <param name="Element">The element to be search</param>
        /// <returns>Returns a bool indicating if the element was found and removed or not</returns>
        bool Contains(T Element);
        /// <summary>
        /// Checks if the element exists in the current set. This element will be on the first nesting level.
        /// </summary>
        /// <param name="tree">The element to be checked</param>
        /// <returns>Returns true if the it is in the set</returns>
        bool Contains(ISetTree<T> tree);
        /// <summary>
        /// Checks if the given set is a subset of the current set. If they have the same cardinality and all elements in the set are in setB. 
        /// Then the setType will be Proper set &amp; same set otherwise it will be a Proper set.
        /// </summary>
        /// <param name="setB">Set that is to contain the current set.</param>
        /// <param name="type">The set type between the current set and setB</param>
        /// <returns>A boolean to indeicate if a set is a subset or not.</returns>
        bool IsSubSetOf(IStructuredSet<T> setB, out SetResultType type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setB"></param>
        /// <returns></returns>
        bool IsElementOf(IStructuredSet<T> setB);
        /// <summary>
        /// Clears the entire set tree.
        /// </summary>
        void Clear();
        /// <summary>
        /// Gets the element in the current set based on an index. The element can be in the root or nested in a subset.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>An element struct which contains the element <typeparamref name="T"/> and position on the set.</returns>
        //Element<T> GetElementByIndex(int index);
    }//interface : IStructuredSet
}//class
