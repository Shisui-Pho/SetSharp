/*
 * File: ISortedSubSets.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISortedSubSets<T> interface, which extends ISortedSetCollection<T> 
 * and IEnumerable<ISetTree<T>>. The interface represents a collection of sorted subsets 
 * (set trees) and provides functionality for adding, removing, and searching within the collection. 
 * Elements within the subsets are required to implement the IComparable<T> interface to maintain 
 * the sorted structure.
 * 
 * Key Features:
 * - Extends ISortedSetCollection<T> and allows enumeration of set trees.
 * - Supports dynamic addition of individual set trees or a range of set trees while preserving order.
 * - Provides methods to check for the existence of a set tree and retrieve its index.
 * - Enables removal of set trees by value or by index.
 */

using SetsLibrary.Interfaces;

namespace SetLibrary.Collections;

/// <summary>
/// Represents a collection of sorted subsets, allowing for addition, removal, and searching of set trees within the collection.
/// </summary>
/// <typeparam name="T">The type of elements in the subsets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISortedSubSets<T> : ISortedSetCollection<T, ISetTree<T>>
    where T : IComparable<T>
{
    /// <summary>
    /// Removes a tree from the sorted list.
    /// </summary>
    /// <param name="val">The tree to be removed.</param>
    /// <returns>True if the tree was successfully removed; otherwise, false.</returns>
    bool Remove(ISetTree<T> val);

    /// <summary>
    /// Checks if a particular tree is contained in the current collection.
    /// </summary>
    /// <param name="val">The tree to be found.</param>
    /// <returns>True if the tree is found; otherwise, false.</returns>
    bool Contains(ISetTree<T> val);

    /// <summary>
    /// Returns the index of the specified tree in the collection.
    /// </summary>
    /// <param name="val">The tree whose index is to be found.</param>
    /// <returns>The index of the tree if found; otherwise, -1.</returns>
    int IndexOf(ISetTree<T> val);

    /// <summary>
    /// Adds a tree to the collection in sorted order.
    /// </summary>
    /// <param name="value">The tree to be added, of type <typeparamref name="ISetTree{T}"/>.</param>
    void Add(ISetTree<T> value);

    /// <summary>
    /// Adds a range of trees to the collection in sorted order.
    /// </summary>
    /// <param name="coll">The collection of trees to be added.</param>
    void AddRange(IEnumerable<ISetTree<T>> coll);
} // interface ISortedSubSets
// namespace SetLibrary.Collections