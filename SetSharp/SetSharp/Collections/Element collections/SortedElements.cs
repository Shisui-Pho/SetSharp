/*
 * File: SortedElements.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the SortedElements<T> class, which provides a sorted collection 
 * of elements that supports operations such as addition, removal, and searching while 
 * maintaining the sorted order of elements. It extends the BaseSortedCollection<T> class 
 * and adheres to the ISortedElements<T> interface.
 * 
 * Key Features:
 * - Inherits all core sorted collection functionalities from BaseSortedCollection<T>.
 * - Provides constructors for initializing the collection with or without an existing set of elements.
 * - Supports efficient operations while ensuring elements remain sorted.
 * - Implements the ISortedElements<T> interface for collection-specific behavior.
 * 
 * Dependencies:
 * - SetSharp.Collections.BaseSortedCollection<T> for base functionality.
 * - SetSharp.Interfaces.ISortedElements<T> for interface adherence.
 * - System.Collections for IEnumerable support.
 */

namespace SetSharp.Collections;

/// <summary>
/// Represents a collection of sorted elements that allows for addition, removal, and searching within the collection.
/// </summary>
/// <typeparam name="T">The type of elements in the collection, which must implement <see cref="IComparable{T}"/>.</typeparam>
public class SortedElements<T> : SortedCollection<T>, ISortedElements<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SortedElements{T}"/> class.
    /// </summary>
    public SortedElements() : base() { }//ctor main

    /// <summary>
    /// Initializes a new instance of the <see cref="SortedElements{T}"/> class with an existing set of elements.
    /// </summary>
    /// <param name="elements">The collection of elements to initialize the sorted collection with.</param>
    public SortedElements(IEnumerable<T> elements)
        : base(elements) { }//ctor 01
}//class
 //namespace SetLibrary.Collections