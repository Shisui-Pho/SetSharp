/*
 * File: IIndexedRedBlackTree.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 January 2025
 * 
 * Description:
 * This file defines the IIndexedRedBlackTree<TElement> interface, which represents an indexed red-black tree data structure. 
 * The interface provides functionality for adding, removing, and querying elements by their index, as well as supporting common operations 
 * like checking if an element is contained in the tree, clearing the tree, and finding the maximum and minimum elements.
 * The elements in the tree are required to implement the IComparable<TElement> interface to ensure proper sorting and ordering.
 * 
 * Key Features:
 * - Implements a red-black tree with indexing support, allowing efficient access to elements by index.
 * - Provides methods for adding, removing, and querying elements within the tree.
 * - Supports retrieving the count of elements and checking if the tree is empty.
 * - Provides access to the minimum and maximum elements in the tree.
 * - Supports enumerating over the elements in the tree.
 */

namespace SetSharp.Collections;

/// <summary>
/// Represents a red-black tree data structure that supports indexed access to elements.
/// </summary>
/// <typeparam name="TElement">The type of elements in the tree. The elements must implement <see cref="IComparable{TElement}"/> for sorting.</typeparam>
public interface IIndexedRedBlackTree<TElement> where TElement : IComparable<TElement>
{
    /// <summary>
    /// Gets the element at the specified index in the tree.
    /// </summary>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    TElement this[int index] { get; }

    /// <summary>
    /// Gets the number of elements in the red-black tree.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Gets a value indicating whether the tree is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Adds an element to the tree.
    /// </summary>
    /// <param name="item">The element to be added to the tree.</param>
    void Add(TElement item);
    /// <summary>
    /// Adds a collection of elements to the tree.
    /// </summary>
    /// <param name="items">The collection of elements to be added to the tree.</param>
    void AddRange(IEnumerable<TElement> items);

    /// <summary>
    /// Clears all elements from the tree.
    /// </summary>
    void Clear();

    /// <summary>
    /// Checks whether the tree contains the specified element.
    /// </summary>
    /// <param name="item">The element to search for in the tree.</param>
    /// <returns>True if the element is found; otherwise, false.</returns>
    bool Contains(TElement item);

    /// <summary>
    /// Returns an enumerator that iterates through the elements of the tree.
    /// </summary>
    /// <returns>An enumerator for the elements in the tree.</returns>
    IEnumerator<TElement> GetEnumerator();

    /// <summary>
    /// Returns the index of the specified element in the tree.
    /// </summary>
    /// <param name="item">The element whose index is to be found.</param>
    /// <returns>The index of the element if found; otherwise, -1.</returns>
    int IndexOf(TElement item);

    /// <summary>
    /// Gets the maximum element in the tree.
    /// </summary>
    /// <returns>The maximum element in the tree.</returns>
    TElement Max();

    /// <summary>
    /// Gets the minimum element in the tree.
    /// </summary>
    /// <returns>The minimum element in the tree.</returns>
    TElement Min();

    /// <summary>
    /// Removes the specified element from the tree.
    /// </summary>
    /// <param name="item">The element to remove from the tree.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    bool Remove(TElement item);

    /// <summary>
    /// Removes the element at the specified index from the tree.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <returns>The element that was removed from the tree.</returns>
    TElement RemoveAt(int index);

    /// <summary>
    /// Attempts to remove the specified element from the tree and returns the removed element.
    /// </summary>
    /// <param name="item">The element to remove from the tree.</param>
    /// <param name="removedData">The removed element, or null if the element was not found.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    bool TryRemove(TElement item, out TElement? removedData);
}