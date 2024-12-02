/*
 * File: ISetCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISetCollection<T> interface, which represents a collection of sets 
 * with various methods for manipulation, enumeration, and querying. Each set in the collection 
 * is structured and strongly typed, ensuring type safety and consistency across operations. 
 * The interface supports indexing, name-based searches, and operations for adding, removing, 
 * and enumerating sets. It also includes methods for resetting and clearing the collection.
 * 
 * Key Features:
 * - Strongly typed collection of sets with type constraints on elements.
 * - Indexer for retrieving sets by position.
 * - Name-based set searching and removal.
 * - Enumerates sets both as structures and as elements.
 * - Supports dynamic manipulation with Add, Remove, Clear, and Reset methods.
 */

using SetLibrary;
using SetsLibrary.Interfaces;

namespace SetsLibrary.Collections;

/// <summary>
/// Represents a collection of sets that can be enumerated and manipulated.
/// </summary>
/// <typeparam name="T">The type of elements in the sets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISetCollection<T> : IEnumerable<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Gets the set at the specified index in the collection.
    /// </summary>
    /// <param name="index">The index of the set in the collection.</param>
    /// <returns>An instance of <see cref="IStructuredSet{T}"/> at the specified index.</returns>
    IStructuredSet<T> this[int index] { get; }

    /// <summary>
    /// Gets the number of sets in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds a new set to the collection.
    /// </summary>
    /// <param name="item">The set to be added.</param>
    void Add(IStructuredSet<T> item);

    /// <summary>
    /// Checks whether the collection contains a specified set.
    /// </summary>
    /// <param name="item">The set to be searched for.</param>
    /// <returns>True if the set is in the collection; otherwise, false.</returns>
    bool Contains(IStructuredSet<T> item);

    /// <summary>
    /// Checks whether the collection contains a set with the specified name.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>True if the set is in the collection; otherwise, false.</returns>
    bool Contains(string name);

    /// <summary>
    /// Finds a set by its name in the collection.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>The set if found; otherwise, null.</returns>
    IStructuredSet<T> FindSetByName(string name);

    /// <summary>
    /// Gets the set structure at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the set.</param>
    /// <returns>A <see cref="IStructuredSet<typeparamref name="T"/>"/> structure of the set at the specified index.</returns>
    IStructuredSet<T> GetSetByIndex(int index);

    /// <summary>
    /// Removes a specified set from the collection.
    /// </summary>
    /// <param name="item">The set to be removed.</param>
    void Remove(IStructuredSet<T> item);

    /// <summary>
    /// Removes a set from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    void Remove(string name);

    /// <summary>
    /// Removes a set at a specified index from the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the set.</param>
    void RemoveAt(int index);

    /// <summary>
    /// Gets an enumerator of the set structures in the collection.
    /// </summary>
    /// <returns>An enumerable collection of the <see cref="IStructuredSet{T}"/> structures.</returns>
    IEnumerator<IStructuredSet<T>> EnumerateWithSetStructure();

    /// <summary>
    /// Resets the naming (numbering) of sets in the collection.
    /// </summary>
    void Reset();

    /// <summary>
    /// Clears the current collection of sets.
    /// </summary>
    void Clear();
} // interface