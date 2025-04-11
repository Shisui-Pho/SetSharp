/*
 * File: ISetCollection.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file defines the ISetCollection<TSet, TType> interface, which represents a collection of sets
 * that can be enumerated, manipulated, and queried. Each set in the collection is strongly typed 
 * and can contain elements of type TType, which must implement <see cref="IComparable{T}"/> for 
 * comparison purposes. The interface supports dynamic operations for adding, removing, and 
 * querying sets, as well as resetting and clearing the collection. It also includes indexing and 
 * name-based searching for easy access to sets.
 * 
 * Key Features:
 * - Strongly typed collection of sets, ensuring type safety across operations.
 * - Support for generic set types and element types.
 * - Indexer for retrieving sets by their name.
 * - Name-based searching and removal of sets.
 * - Enumeration of sets both as structures and as individual elements.
 * - Methods for dynamic manipulation: Add, AddRange, Contains, Remove, Reset, and Clear.
 * - Each element in the set must implement <see cref="IComparable{T}"/> for ordering and comparison.
 */
namespace SetsLibrary.Collections;

/// <summary>
/// Represents a collection of sets that can be enumerated and manipulated.
/// Each set in the collection is indexed by its name and can be accessed or modified using various methods.
/// </summary>
/// <typeparam name="TSet">The type of the set. It must implement <see cref="IStructuredSet{T}"/>.</typeparam>
/// <typeparam name="TType">The type of the elements within the sets, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISetCollection<TSet, TType> : IEnumerable<KeyValuePair<string, TSet>>
{
    /// <summary>
    /// Gets the set associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>An instance of <see cref="IStructuredSet{T}"/> representing the set with the specified name, or null if not found.</returns>
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

    /// <summary>
    /// Adds a range of sets to the collection.
    /// </summary>
    /// <param name="items">A collection of sets to be added.</param>
    void AddRange(IEnumerable<TSet> items);

    /// <summary>
    /// Checks whether the collection contains the specified set.
    /// </summary>
    /// <param name="item">The set to be searched for.</param>
    /// <returns>True if the set is found in the collection, otherwise false.</returns>
    bool Contains(TSet item);

    /// <summary>
    /// Checks whether the collection contains a set with the specified name.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>True if the set with the specified name exists in the collection, otherwise false.</returns>
    bool Contains(string name);

    /// <summary>
    /// Finds a set by its name in the collection.
    /// </summary>
    /// <param name="name">The name of the set.</param>
    /// <returns>The set if found, otherwise null.</returns>
    TSet? FindSetByName(string name);

    /// <summary>
    /// Removes a set from the collection by its name.
    /// </summary>
    /// <param name="name">The name of the set to be removed.</param>
    /// <returns>True if the set was successfully removed, otherwise false.</returns>
    bool Remove(string name);

    /// <summary>
    /// Resets the naming (or numbering) of sets in the collection.
    /// This can be useful for reordering or renaming sets within the collection.
    /// </summary>
    void Reset();

    /// <summary>
    /// Clears the collection of all sets.
    /// </summary>
    void Clear();
}
