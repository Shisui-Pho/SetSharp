/*
 * File: SortedSubSets.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the SortedSubSets<T> class, which implements a sorted collection 
 * of subsets, where each subset is an ISetTree<T>. The class extends the BaseSortedCollection<ISetTree<T>> 
 * to provide functionalities such as maintaining sorted subsets, adding new subsets, 
 * and ensuring type safety with constraints on T to implement IComparable<T>.
 * 
 * Key Features:
 * - Inherits all core sorted collection functionalities from BaseSortedCollection<ISetTree<T>>.
 * - Provides constructors for initializing the collection with or without an existing set of subsets.
 * - Ensures subsets are stored and maintained in sorted order.
 * - Implements the ISortedSubSets<T> interface to adhere to collection-specific contracts.
 * 
 * Dependencies:
 * - SetSharp.Interfaces for ISetTree<T> and ISortedSubSets<T>.
 * - System.Collections for IEnumerable support.
 */

namespace SetSharp.Collections;

/// <summary>
/// Represents a collection of sorted subsets, each being a <see cref="ISetTree{T}"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the subsets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public class SortedSubSets<T> : SortedCollection<ISetTree<T>>, ISortedSubSets<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SortedSubSets{T}"/> class.
    /// </summary>
    public SortedSubSets()
        : base()
    {
    }//ctor main

    /// <summary>
    /// Initializes a new instance of the <see cref="SortedSubSets{T}"/> class with an existing set of subsets.
    /// </summary>
    /// <param name="subSets">A collection of subsets to initialize the sorted collection with.</param>
    public SortedSubSets(IEnumerable<ISetTree<T>> subSets)
        : base(subSets)
    {
    }//ctor 01
}//class