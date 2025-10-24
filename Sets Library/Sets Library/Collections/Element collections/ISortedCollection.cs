/*
 * File: ISortedSetCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISortedSetCollection<T, TEnumerator> interface, which represents a collection 
 * of sorted sets. The interface provides functionality for manipulating and querying the collection, 
 * including retrieving the count of elements and removing elements by index. Elements in the collection 
 * are required to implement the IComparable interface, ensuring effective sorting and comparison.
 * 
 * Key Features:
 * - Represents a collection of sorted sets with type constraints on elements.
 * - Provides a property to retrieve the total count of elements in the collection.
 * - Allows dynamic removal of elements by their index.
 * - Supports enumeration over a specified type of enumerator for the collection.
 */

namespace SetsLibrary.Collections;

/// <summary>
/// Represents a collection of sorted sets that can be manipulated and enumerated.
/// </summary>
/// <typeparam name="TElementContained">The type of enumerator/structure used to iterate through the collection.</typeparam>
public interface ISortedCollection<TElementContained> : IEnumerable<TElementContained>
    where TElementContained : IComparable<TElementContained>
{
    /// <summary>
    /// Gets an element in the collection on the specified index.
    /// </summary>
    /// <param name="index">The zero based index.</param>
    /// <returns>The element on the specified index</returns>
    TElementContained this[int index] { get; }

    /// <summary>
    /// Gets the number of elements within the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Removes an element from the current collection based on the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to be removed.</param>
    void RemoveAt(int index);

    /// <summary>
    /// Removes an element from the sorted list.
    /// </summary>
    /// <param name="val">The element to be removed.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    bool Remove(TElementContained val);

    /// <summary>
    /// Checks if a particular element is contained in the current collection.
    /// </summary>
    /// <param name="val">The element to be found.</param>
    /// <returns>True if the element is found; otherwise, false.</returns>
    bool Contains(TElementContained val);

    /// <summary>
    /// Returns the index of the specified element in the collection.
    /// </summary>
    /// <param name="val">The element whose index is to be found.</param>
    /// <returns>The index of the element if found; otherwise, -1.</returns>
    int IndexOf(TElementContained val);


    /// <summary>
    /// Adds an element to the collection in sorted order. Repeated elements will be added.
    /// </summary>
    /// <param name="value">The value to be added, of type <typeparamref name="TElementContained"/>.</param>
    void AddIfDuplicate(TElementContained value);

    /// <summary>
    /// Adds an element to the collection in sorted order and ignore duplicates
    /// </summary>
    /// <param name="val">The value to be added, of type <typeparamref name="TElementContained"/></param>
    void Add(TElementContained val);

    /// <summary>
    /// Adds a range of elements to the collection in sorted order.
    /// </summary>
    /// <param name="coll">The collection of elements to be added.</param>
    void AddRange(IEnumerable<TElementContained> coll);

    /// <summary>
    /// Clears the current collection.
    /// </summary>
    void Clear();
} // interface
