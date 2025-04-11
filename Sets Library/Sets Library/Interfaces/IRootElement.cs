/*
 * File: IRootElement.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 11 April 2025
 * 
 * Description:
 * This file contains the definition of the IRootElement interface, 
 * which establishes a contract for managing root-level elements within a set.
 * It provides methods for retrieving, adding, removing, and indexing 
 * elements that are considered "root elements" in the context of the set.
 * 
 * Key Features:
 * - Supports enumeration of root elements.
 * - Allows adding and removing single or multiple elements.
 * - Provides indexing capabilities for element lookup.
 * - Generic and type-safe with support for any element type.
 */

namespace SetsLibrary;

/// <summary>
/// Defines a contract for managing the root elements of a set. 
/// This interface supports enumeration, addition, removal, and indexing 
/// of root-level elements within a set collection.
/// </summary>
/// <typeparam name="TElement">The type of the elements contained in the set.</typeparam>
public interface IRootElement<TElement> : ISetTreeElement<TElement>
    where TElement : IComparable<TElement>
{
    /// <summary>
    /// Gets the count of root elements in the current set.
    /// </summary>
    /// <value>
    /// The total number of root elements contained in the set.
    /// </value>
    int CountRootElements { get; }

    /// <summary>
    /// Returns an enumerator that iterates through the root elements of the current set.
    /// </summary>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> that can be used to iterate through the root elements.
    /// </returns>
    IEnumerable<TElement> GetRootElementsEnumerator();
} // interface