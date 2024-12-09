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
namespace SetsLibrary.Collections;

/// <summary>
/// Represents a collection of sets that can be enumerated and manipulated.
/// </summary>
/// <typeparam name="T">The type of elements in the sets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISetCollection<TSet, TType> : IEnumerable<KeyValuePair<string,TSet>>
    where TType : IComparable<TType>
{
    /// <summary>
    /// Gets the set at the specified index in the collection.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>An instance of <see cref="IStructuredSet{T}"/> at the specified index.</returns>
    TSet? this[string name] { get; }

    /// <summary>
    /// Gets the number of sets in the collection.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds a new set to the collection.
    /// </summary>
    /// <param name="item">The set to be added.</param>
    void Add(TSet item);
    void AddRange(IEnumerable<TSet> items);
    /// <summary>
    /// Checks whether the collection contains a specified set.
    /// </summary>
    /// <param name="item">The set to be searched for.</param>
    /// <returns>True if the set is in the collection; otherwise, false.</returns>
    bool Contains(TSet item);

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
    TSet? FindSetByName(string name);
    /// <summary>
    /// Removes a set from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    bool Remove(string name);

    /// <summary>
    /// Resets the naming (numbering) of sets in the collection.
    /// </summary>
    void Reset();

    /// <summary>
    /// Clears the current collection of sets.
    /// </summary>
    void Clear();
} // interface