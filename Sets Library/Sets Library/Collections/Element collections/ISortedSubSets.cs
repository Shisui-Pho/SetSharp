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
namespace SetsLibrary.Collections;

/// <summary>
/// Represents a collection of sorted subsets, allowing for addition, removal, and searching of set trees within the collection.
/// </summary>
/// <typeparam name="T">The type of elements in the subsets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISortedSubSets<T> : ISortedCollection<ISetTree<T>>
    where T : IComparable<T>
{
} // interface ISortedSubSets