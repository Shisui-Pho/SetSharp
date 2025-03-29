/*
 * File: IStructuredSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 30 December 2024
 * 
 * Description:
 * This file defines the IStructuredSet interface, providing methods and properties for managing
 * structured sets containing elements and nested subsets. It enables operations like adding,
 * removing, and checking for elements or subsets, as well as managing cardinality and set relationships.
 * 
 * Key Features:
 * - Allows adding and removing elements or nested subsets.
 * - Supports checking relationships between sets (e.g., subset checks, element inclusion).
 * - Enables working with cardinality (set size) and extraction configurations.
 * - Handles nested subsets and allows element retrieval via indexers.
 * 
 * The generic type parameter T must implement the IComparable<T> interface, ensuring that elements
 * within the set can be compared to one another.
 */

namespace SetsLibrary;

/// <summary>
/// Defines an interface for managing structured sets with operations on both elements and nested subsets. 
/// This interface supports various set manipulation functions, such as element and subset management, 
/// relationship checks, and extraction configurations.
/// </summary>
/// <typeparam name="T">The type of elements in the set. T must implement IComparable&lt;T&gt;.</typeparam>
public interface IStructuredSet<T> where T : IComparable<T>
{
    /// <summary>
    /// Gets the original string expression representing the set.
    /// </summary>
    string OriginalExpression { get; }

    /// <summary>
    /// Gets the cardinality (number of elements) of the evaluated set.
    /// </summary>
    int Cardinality { get; }

    /// <summary>
    /// Gets the current settings for extracting elements from the set.
    /// </summary>
    SetExtractionConfiguration ExtractionConfiguration { get; }

    /// <summary>
    /// Adds an element to the set. If the element already exists, it will not be added.
    /// </summary>
    /// <param name="Element">The element to be added to the set.</param>
    void AddElement(T Element);

    /// <summary>
    /// Adds a new subset to the set. If the subset already exists, it will not be added. 
    /// This subset will be at the first nesting level.
    /// </summary>
    /// <param name="subset">The subset to add to the set.</param>
    void AddElement(IStructuredSet<T> subset);

    /// <summary>
    /// Adds a new subset to the set, represented as a string. 
    /// The string will be treated and extracted as a subset.
    /// </summary>
    /// <param name="subset">A string representation of the subset to be added.</param>
    void AddSubsetAsString(string subset);

    /// <summary>
    /// Removes an element (subset or individual element) from the first nesting level of the set.
    /// If the element is not found, no action is taken.
    /// </summary>
    /// <param name="subset">The subset (or element) to remove from the set.</param>
    /// <returns>True if the element was found and removed; otherwise, false.</returns>
    bool RemoveElement(IStructuredSet<T> subset);

    /// <summary>
    /// Merges the current set with another set, adding the elements from the other set at the first nesting level.
    /// </summary>
    /// <param name="set">The set to merge with the current set.</param>
    /// <returns>A new set containing elements from both sets.</returns>
    IStructuredSet<T> MergeWith(IStructuredSet<T> set);

    /// <summary>
    /// Removes all elements from the current set that are present in another set.
    /// </summary>
    /// <param name="setB">The set whose elements should be removed from the current set.</param>
    /// <returns>A new set without elements found in setB.</returns>
    IStructuredSet<T> Without(IStructuredSet<T> setB);

    /// <summary>
    /// Removes an element from the current set. The element must be at the first nesting level.
    /// </summary>
    /// <param name="Element">The element to remove.</param>
    /// <returns>True if the element was found and removed; otherwise, false.</returns>
    bool RemoveElement(T Element);

    /// <summary>
    /// Checks whether an element exists in the set. The element must be at the first nesting level.
    /// </summary>
    /// <param name="Element">The element to check for existence in the set.</param>
    /// <returns>True if the element exists in the set; otherwise, false.</returns>
    bool Contains(T Element);

    /// <summary>
    /// Checks whether a specific subset exists in the current set at the first nesting level.
    /// </summary>
    /// <param name="subset">The subset to check for.</param>
    /// <returns>True if the subset exists in the set; otherwise, false.</returns>
    bool Contains(IStructuredSet<T> subset);

    /// <summary>
    /// Checks if the current set is a subset of another set.
    /// It also identifies the relationship type (e.g., Proper Set, Same Set, etc.) between the sets.
    /// </summary>
    /// <param name="setB">The set to check for subset inclusion.</param>
    /// <param name="type">Outputs the set relationship type between the current set and setB.</param>
    /// <returns>True if the current set is a subset of setB; otherwise, false.</returns>
    bool IsSubSetOf(IStructuredSet<T> setB, out SetResultType type);

    /// <summary>
    /// Checks if the current set is an element of another set.
    /// </summary>
    /// <param name="setB">The set to check for element inclusion.</param>
    /// <returns>True if the current set is an element of setB; otherwise, false.</returns>
    bool IsElementOf(IStructuredSet<T> setB);

    /// <summary>
    /// Clears all elements and subsets from the set, effectively resetting it to an empty state.
    /// </summary>
    void Clear();

    /// <summary>
    /// Builds and returns a string representation of the structured set.
    /// </summary>
    /// <returns>A string representation of the set.</returns>
    string BuildStringRepresentation();

    /// <summary>
    /// Enumerates and returns all root elements in the current set.
    /// </summary>
    /// <returns>An enumerable collection of root elements in the set.</returns>
    IEnumerable<T> EnumerateRootElements();

    /// <summary>
    /// Enumerates and returns all subsets in the current set.
    /// </summary>
    /// <returns>An enumerable collection of subsets in the set as a set.</returns>
    IEnumerable<IStructuredSet<T>> EnumerateSubsets();

    /// <summary>
    /// Builds and returns a new, empty set.
    /// </summary>
    /// <returns>A new, empty instance of a structured set.</returns>
    IStructuredSet<T> BuildNewSet();

    /// <summary>
    /// Builds and returns a new set based on the provided string representation.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of a structured set based on the provided string.</returns>
    IStructuredSet<T> BuildNewSet(string setString);
} // interface : IStructuredSet
// namespace SetsLibrary
