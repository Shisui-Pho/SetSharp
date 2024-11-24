/*
 * File: ISortedElements.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISortedElements<T> interface, which extends ISortedSetCollection<T> 
 * and IEnumerable<T>. The interface represents a collection of sorted elements, providing 
 * functionality for adding, removing, and searching elements within the collection. The elements 
 * are required to implement the IComparable interface to ensure proper sorting behavior.
 * 
 * Key Features:
 * - Extends ISortedSetCollection<T> and supports enumeration of elements.
 * - Allows dynamic addition of single elements or a range of elements while maintaining sorting.
 * - Provides methods for searching elements by value and retrieving their index.
 * - Supports removal of elements by value or by index.
 */

namespace SetLibrary.Collections;
/// <summary>
/// Represents a collection of sorted elements that allows for addition, removal, and searching within the collection.
/// </summary>
/// <typeparam name="T">The type of elements in the collection, which must implement <see cref="IComparable"/>.</typeparam>
public interface ISortedElements<T> : ISortedSetCollection<T>, IEnumerable<T>
    where T : IComparable
{
    /// <summary>
    /// Removes an element from the sorted list.
    /// </summary>
    /// <param name="val">The element to be removed.</param>
    /// <returns>True if the element was successfully removed; otherwise, false.</returns>
    bool Remove(T val);

    /// <summary>
    /// Checks if a particular element is contained in the current collection.
    /// </summary>
    /// <param name="val">The element to be found.</param>
    /// <returns>True if the element is found; otherwise, false.</returns>
    bool Contains(T val);

    /// <summary>
    /// Returns the index of the specified element in the collection.
    /// </summary>
    /// <param name="val">The element whose index is to be found.</param>
    /// <returns>The index of the element if found; otherwise, -1.</returns>
    int IndexOf(T val);

    /// <summary>
    /// Adds an element to the collection in sorted order.
    /// </summary>
    /// <param name="value">The value to be added, of type <typeparamref name="T"/>.</param>
    void Add(T value);

    /// <summary>
    /// Adds a range of elements to the collection in sorted order.
    /// </summary>
    /// <param name="coll">The collection of elements to be added.</param>
    void AddRange(IEnumerable<T> coll);
} // interface ISortedElements
// namespace SetLibrary.Collections