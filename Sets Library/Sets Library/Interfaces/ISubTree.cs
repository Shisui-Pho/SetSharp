/*
 * File: ISubTree.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 11 April 2025
 * 
 * Description:
 * This file contains the definition of the ISubTree interface, 
 * which provides a contract for managing nested subsets within a set.
 * It allows operations such as retrieving, adding, removing, and indexing 
 * subsets that are part of the current set, supporting hierarchical set structures.
 * 
 * Key Features:
 * - Supports enumeration of nested subsets within a set.
 * - Allows adding and removing single or multiple subsets.
 * - Provides indexing capabilities for subset lookup.
 * - Generic and type-safe with support for set trees.
 */

namespace SetsLibrary;

/// <summary>
/// Defines a contract for managing nested subsets (or subtrees) within a set.
/// This interface provides methods for adding, removing, and indexing subsets, 
/// allowing hierarchical structures within the set collection.
/// </summary>
/// <typeparam name="T">The type of the set tree, which must implement <see cref="ISetTree{T}"/> and <see cref="IComparable{T}"/>.</typeparam>
public interface ISubTree<T>
    where T : ISetTree<T>, IComparable<T>
{
    /// <summary>
    /// Gets the count of subsets contained within the current set.
    /// </summary>
    /// <value>
    /// The number of nested subsets within the set.
    /// </value>
    int CountSubsets { get; }

    /// <summary>
    /// Returns an enumerator that iterates through the subsets of the current set.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{ISetTree{T}}"/> that can be used to iterate through the subsets.
    /// </returns>
    IEnumerable<ISetTree<T>> GetSubsetsEnumerator();

    /// <summary>
    /// Adds a subset (tree) inside the current set.
    /// </summary>
    /// <param name="tree">The tree representation of the subset to be added to the set.</param>
    void AddSubSetTree(ISetTree<T> tree);

    /// <summary>
    /// Adds a range of subsets to the current set.
    /// </summary>
    /// <param name="subsets">A collection of subsets to be added to the set.</param>
    void AddRange(IEnumerable<ISetTree<T>> subsets);

    /// <summary>
    /// Removes a subset from the current set.
    /// </summary>
    /// <param name="element">The subset to be removed from the set.</param>
    /// <returns>
    /// A boolean value indicating whether the subset was successfully removed.
    /// </returns>
    bool RemoveElement(ISetTree<T> element);

    /// <summary>
    /// Gets the index of the specified subset in the nested sets of the current set.
    /// </summary>
    /// <param name="subset">The subset to search for within the current set.</param>
    /// <returns>
    /// The zero-based index of the subset if found; otherwise, -1.
    /// </returns>
    int IndexOf(ISetTree<T> subset);
} // interface
// namespace
