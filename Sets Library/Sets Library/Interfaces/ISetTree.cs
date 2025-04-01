/*
 * File: ISetTree.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 November 2024
 * 
 * Description:
 * Defines the ISetTree<T> interface, which represents a tree-based structure for managing sets. 
 * This interface provides methods for manipulating elements and subsets, retrieving sorted elements, 
 * and enumerating subsets and root elements.
 * 
 * Key Features:
 * - Tree-based set structure supporting nested subsets and root element management.
 * - Methods for adding, removing, and retrieving elements and subsets.
 * - Enumerator support for iterating through subsets and root elements.
 * - Provides cardinality, subset count, and extraction configuration details.
 * - Supports custom string representation and element index lookup.
 */

namespace SetsLibrary;

/// <summary>
/// Represents a tree-based set structure, where elements can be stored in both root and subset collections. 
/// This interface provides methods to manipulate, retrieve, and manage elements and subsets in a sorted set tree.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISetTree<T> : IComparable<ISetTree<T>>
    where T : IComparable<T>
{
    /// <summary>
    /// Gets the string representation of the root elements in the set.
    /// </summary>
    /// <value>
    /// A string representing the root elements of the set.
    /// </value>
    string RootElements { get; }

    /// <summary>
    /// Gets the size of the current set, i.e., the total number of elements.
    /// </summary>
    /// <value>
    /// The number of elements in the current set.
    /// </value>
    int Count { get; }
    
    /// <summary>
    /// Gets the count of root elements in the current set.
    /// </summary>
    /// <value>
    /// The count of root elements in the set.
    /// </value>
    int CountRootElements { get; }

    /// <summary>
    /// Gets the count of subsets contained within the current set.
    /// </summary>
    /// <value>
    /// The number of nested subsets within the set.
    /// </value>
    int CountSubsets { get; }

    /// <summary>
    /// Gets the set extraction settings associated with the current set.
    /// </summary>
    /// <value>
    /// The <see cref="SetsConfigurations"/> settings for extracting the set.
    /// </value>
    SetsConfigurations ExtractionSettings { get; }

    /// <summary>
    /// Gets the information of the null elements in the root of the set. All null elements
    ///  within a nested subsets will be ignored for the current set.
    /// </summary>
    SetTreeInfo TreeInfo { get; }
    /// <summary>
    /// Returns an enumerator that iterates through the subsets of the current set.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the subsets of the current set.
    /// </returns>
    IEnumerable<ISetTree<T>> GetSubsetsEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through the root elements of the current set.
    /// </summary>
    /// <returns>
    /// An enumerator that can be used to iterate through the root elements of the current set.
    /// </returns>
    IEnumerable<T> GetRootElementsEnumerator();

    /// <summary>
    /// Adds a subset inside the current set.
    /// </summary>
    /// <param name="tree">The tree representation of the subset to be added.</param>
    void AddSubSetTree(ISetTree<T> tree);

    /// <summary>
    /// Adds a single element to the root elements of the current set.
    /// </summary>
    /// <param name="element">The element to be added to the root.</param>
    void AddElement(T element);

    /// <summary>
    /// Adds a range of elements to the root elements of the current set.
    /// </summary>
    /// <param name="elements">The elements to be added.</param>
    void AddRange(IEnumerable<T> elements);

    /// <summary>
    /// Adds a range of subsets to the current set.
    /// </summary>
    /// <param name="subsets">The subsets to be added.</param>
    void AddRange(IEnumerable<ISetTree<T>> subsets);

    /// <summary>
    /// Removes an element from the root elements of the current set.
    /// </summary>
    /// <param name="element">The element to be removed.</param>
    /// <returns>
    /// A boolean value indicating whether the element was successfully removed.
    /// </returns>
    bool RemoveElement(T element);

    /// <summary>
    /// Removes a subset from the current set.
    /// </summary>
    /// <param name="element">The subset to be removed.</param>
    /// <returns>
    /// A boolean value indicating whether the subset was successfully removed.
    /// </returns>
    bool RemoveElement(ISetTree<T> element);

    /// <summary>
    /// Gets the index of the specified element in the root of the current set.
    /// </summary>
    /// <param name="element">The element to search for.</param>
    /// <returns>
    /// The zero-based index of the element within the root, or -1 if the element is not found.
    /// </returns>
    int IndexOf(T element);

    /// <summary>
    /// Gets the index of the specified subset in the nested sets of the current set.
    /// </summary>
    /// <param name="subset">The subset to search for.</param>
    /// <returns>
    /// The zero-based index of the subset within the current set, or -1 if the subset is not found.
    /// </returns>
    int IndexOf(ISetTree<T> subset);

    /// <summary>
    /// Gets the string representation of the current set tree.
    /// </summary>
    /// <returns>
    /// A string representing the current set and its structure.
    /// </returns>
    string ToString();
}
//namespace