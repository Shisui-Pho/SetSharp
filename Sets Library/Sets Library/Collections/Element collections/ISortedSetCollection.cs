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
/// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{T}"/>.</typeparam>
/// <typeparam name="TEnumerator">The type of enumerator used to iterate through the collection.</typeparam>
public interface ISortedSetCollection<TElement, TEnumerator> : IEnumerable<TEnumerator>
    where TElement : IComparable<TElement>
{
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
    /// Clears the current collection.
    /// </summary>
    void Clear();
} // interface
