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

namespace SetsLibrary.Collections;
/// <summary>
/// Represents a collection of sorted elements that allows for addition, removal, and searching within the collection.
/// </summary>
/// <typeparam name="T">The type of elements in the collection, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISortedElements<T> : ISortedCollection<T>
    where T : IComparable<T>
{
} // interface ISortedElements
