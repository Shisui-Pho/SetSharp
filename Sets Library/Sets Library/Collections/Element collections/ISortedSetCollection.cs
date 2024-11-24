/*
 * File: ISortedSetCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISortedSetCollection<T> interface, which represents a collection 
 * of sorted sets. The interface provides basic functionality for manipulating and querying 
 * the collection, including retrieving the count of elements and removing elements by index. 
 * Elements in the collection are required to implement the IComparable interface, ensuring 
 * that they can be sorted and compared effectively.
 * 
 * Key Features:
 * - Represents a collection of sorted sets with type constraints on elements.
 * - Provides a property to retrieve the total count of elements in the collection.
 * - Allows dynamic removal of elements by their index.
 */

namespace SetLibrary.Collections;

/// <summary>
/// Represents a collection of sorted sets that can be manipulated and enumerated.
/// </summary>
/// <typeparam name="T">The type of elements in the sets, which must implement <see cref="IComparable"/>.</typeparam>
public interface ISortedSetCollection<T>
    where T : IComparable<T>
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
} // interface