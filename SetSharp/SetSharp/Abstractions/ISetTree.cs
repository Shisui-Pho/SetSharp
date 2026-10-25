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

namespace SetSharp;

/// <summary>
/// Represents a tree-based set structure, where elements can be stored in both root and subset collections. 
/// This interface provides methods to manipulate, retrieve, and manage elements and subsets in a sorted set tree.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/>.</typeparam>
public interface ISetTree<T> : IRootElement<T>, ISubTree<T> ,IComparable<ISetTree<T>>
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
    /// Gets the string representation of the current set tree.
    /// </summary>
    /// <returns>
    /// A string representing the current set and its structure.
    /// </returns>
    string ToString();
}
//namespace