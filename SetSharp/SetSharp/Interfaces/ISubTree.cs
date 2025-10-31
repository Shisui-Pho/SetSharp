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

namespace SetSharp;

/// <summary>
/// Defines a contract for managing nested subsets (or subtrees) within a set.
/// This interface provides methods for adding, removing, and indexing subsets, 
/// allowing hierarchical structures within the set collection.
/// </summary>
/// <typeparam name="T">The type of the set tree, which must implement <see cref="ISetTree{T}"/> and <see cref="IComparable{T}"/>.</typeparam>
public interface ISubTree<T> : ISetTreeElement<ISetTree<T>>
    where T : IComparable<T>
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
    /// An <see cref="IEnumerable{T}"/> that can be used to iterate through the subsets.
    /// </returns>
    IEnumerable<ISetTree<T>> GetSubsetsEnumerator();
} // interface
// namespace
