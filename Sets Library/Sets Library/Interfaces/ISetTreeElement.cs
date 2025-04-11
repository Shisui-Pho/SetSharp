/*
 * File: ISetTreeElement.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 11 April 2025
 * 
 * Description:
 * This file contains the definition of the ISetTreeElement interface, 
 * which serves as an abstract base for set tree elements. It provides a 
 * set of common methods for managing elements and subsets within a set, 
 * including adding, removing, and indexing elements.
 * 
 * Key Features:
 * - Provides common operations for adding and removing elements.
 * - Supports indexing of elements within the set.
 * - Serves as a base for both root elements and subsets, which extend this functionality.
 * - Generic and type-safe, where TElement must be comparable.
 * 
 * Type Parameters:
 * - TElement: The type of the elements within the set, which must implement <see cref="IComparable{TElement}"/>.
 */

namespace SetsLibrary;

/// <summary>
/// Defines common methods for managing elements and subsets in a set tree, including 
/// adding, removing, and indexing elements. The type parameter <typeparamref name="TElement"/> 
/// must implement <see cref="IComparable{TElement}"/>.
/// </summary>
/// <typeparam name="TElement">The type of the elements within the set, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
public interface ISetTreeElement<TElement> where TElement : IComparable<TElement>
{
    /// <summary>
    /// Adds a single element to the root elements of the current set.
    /// </summary>
    /// <param name="element">The element to be added to the root elements collection.</param>
    void AddElement(TElement element);

    /// <summary>
    /// Adds a range of elements to the root elements of the current set.
    /// </summary>
    /// <param name="elements">A collection of elements to add to the root.</param>
    void AddRange(IEnumerable<TElement> elements);

    /// <summary>
    /// Removes an element from the root elements of the current set.
    /// </summary>
    /// <param name="element">The element to remove from the root.</param>
    /// <returns>
    /// <c>true</c> if the element was successfully removed; otherwise, <c>false</c>.
    /// </returns>
    bool RemoveElement(TElement element);

    /// <summary>
    /// Gets the index of the specified element within the root elements.
    /// </summary>
    /// <param name="element">The element to locate in the root elements collection.</param>
    /// <returns>
    /// The zero-based index of the element if found; otherwise, -1.
    /// </returns>
    int IndexOf(TElement element);
} // interface
// namespace
